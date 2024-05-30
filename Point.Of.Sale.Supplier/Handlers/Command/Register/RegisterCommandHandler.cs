using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Supplier.Repository;
using Polly;

namespace Point.Of.Sale.Supplier.Handlers.Command.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IRepository _repository;

    public RegisterCommandHandler(IRepository repository, ILogger<RegisterCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Add(new Persistence.Models.Supplier
        {
            TenantId = request.TenantId,
            Name = request.Name,
            Address = request.Address,
            Phone = request.Phone,
            Email = request.Email,
            City = request.City,
            State = request.State,
            Country = request.Country,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow,
            Active = true,
            UpdatedBy = "User",
        }, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<string>().FromException(result.FinalException),
            _ => ResultsTo.Something(result.Result!.Value),
        };
    }
}
