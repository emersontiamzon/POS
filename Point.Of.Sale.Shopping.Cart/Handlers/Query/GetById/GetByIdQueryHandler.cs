using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Shopping.Cart.Repository;
using Polly;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, CartResponse>
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository, ILogger<GetByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<CartResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.Id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<CartResponse>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<CartResponse>().WithMessage("Shopping Cart not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<CartResponse>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<CartResponse>().FromResults(result.Result),
            _ => ResultsTo.Success(new CartResponse
            {
                Id = result.Result!.Value.Id,
                CustomerId = result.Result.Value.CustomerId,
                ItemCount = result.Result.Value.ItemCount,
                Active = result.Result.Value.Active,
                CreatedOn = result.Result.Value.CreatedOn,
                UpdatedOn = result.Result.Value.UpdatedOn,
                TenantId = result.Result.Value.TenantId,
            }),
        };
    }
}
