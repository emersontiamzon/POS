using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Sales.Handlers.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly ILogger<UpdateCommandHandler> _logger;
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository, ILogger<UpdateCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Update(new Persistence.Models.Sale
        {
            Id = request.Id,
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            LineItems = request.LineItems,
            SubTotal = request.SubTotal,
            TotalDiscounts = request.TotalDiscounts,
            TaxPercentage = request.TaxPercentage,
            SalesTax = request.SalesTax,
            TotalSales = request.TotalSales,
            SaleDate = DateTime.UtcNow,
            Active = request.Active,
            Status = request.Status,
        }, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound().WithMessage("Sales Not Found"),
            {Result.Value.Count: 0} => ResultsTo.NotFound().WithMessage("Sales not updated"),
            _ => ResultsTo.Something(result.Result.Value.Count > 0),
        };
    }
}
