using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Repository;
using Polly;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Command.UpsertLineItem;

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
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.UpsertLineItem(new Models.UpsertLineItem
        {
            CartId = request.CartId,
            LineId = request.LineId,
            TenantId = request.TenantId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            ProductDescription = request.ProductDescription,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            LineTotal = request.LineTotal,
        }, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound().WithMessage("Shopping Cart Not Found"),
            {Result.Value.Count: 0} => ResultsTo.NotFound().WithMessage("Shopping Cart line item not upserted"),
            _ => ResultsTo.Something(result.Result!.Value.Count > 0),
        };
    }
}
