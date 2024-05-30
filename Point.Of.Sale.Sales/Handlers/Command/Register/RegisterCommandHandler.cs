using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.Enums;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Sales.Handlers.Command.Register;

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
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Add(new Persistence.Models.Sale
        {
            TenantId = request.TenantId,
            CustomerId = request.CustomerId,
            LineItems = request.LineItems,
            SubTotal = request.SubTotal,
            TaxPercentage = request.TaxPercentage,
            SalesTax = request.SalesTax,
            TotalSales = request.TotalSales,
            SaleDate = request.SaleDate,
            Active = true,
            Status = SaleStatus.OrderPlaced,
        }, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure().FromException(result.FinalException),
            _ => ResultsTo.Something(result.Result),
        };
    }
}
