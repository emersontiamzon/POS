using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shopping.Cart.Models;
using Point.Of.Sale.Shopping.Cart.Repository;
using Polly;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<CartResponse>>
{
    private readonly ILogger<GetByTenantIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository, ILogger<GetByTenantIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<List<CartResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetByTenantId(request.id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<List<CartResponse>>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<List<CartResponse>>().WithMessage("Shopping Cart Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<List<CartResponse>>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<List<CartResponse>>().FromResults(result.Result),
            _ => ResultsTo.Success(result.Result.Value.Select(r => new CartResponse
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    ItemCount = r.ItemCount,
                    Active = r.Active,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    TenantId = r.TenantId,
                })
                .ToList()),
        };
    }
}
