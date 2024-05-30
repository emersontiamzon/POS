using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Customer.Handlers.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(IRepository repository, IUnitOfWork unitOfWork, ILogger<RegisterCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Add(new Persistence.Models.Customer
        {
            TenantId = request.TenantId,
            Name = request.Name,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Active = true,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure().FromException(result.FinalException),
            _ => ResultsTo.Something(result),
        };
    }
}
