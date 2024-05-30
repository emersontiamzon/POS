using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Persistence.UnitOfWork;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Inventory.Handlers.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly ILogger<UpdateCommandHandler> _logger;
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCommandHandler(IRepository repository, IUnitOfWork unitOfWork, ILogger<UpdateCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Update(new Persistence.Models.Inventory
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CategoryId = request.CategoryId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Active = request.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound().WithMessage("Inventory Not Found"),
            {Result.Value.Count: 0} => ResultsTo.BadRequest().WithMessage("Inventory not updated"),
            _ => ResultsTo.Something(result.Result.Value.Count > 0),
        };
    }
}