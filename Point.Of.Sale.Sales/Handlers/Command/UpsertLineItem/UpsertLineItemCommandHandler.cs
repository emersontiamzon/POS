using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Sales.Handlers.Command.UpsertLineItem;

public class UpsertLineItemCommandHandler : ICommandHandler<UpsertLineItemCommand>
{
    private readonly ILogger<UpsertLineItemCommandHandler> _logger;
    private readonly IRepository _repository;

    public UpsertLineItemCommandHandler(IRepository repository, ILogger<UpsertLineItemCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults> Handle(UpsertLineItemCommand request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.UpsertLineItem(new UpsertSaleLineItem
        {
            SaleId = request.SaleId,
            LineId = request.LineId,
            TenantId = request.TenantId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            LineDiscount = request.LineDiscount,
            Active = true,
            LineTax = request.LineTax,
            ProductDescription = request.ProductDescription,
            LineTotal = request.LineTax + request.UnitPrice * request.Quantity - request.LineDiscount,
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
