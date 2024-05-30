using MediatR;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Handlers.Query.Extension;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Inventory.Handlers.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<InventoryResponse>>
{
    private readonly ILogger<GetByTenantIdQueryHandler> _logger;
    private readonly IRepository _repository;
    private readonly ISender _sender;

    public GetByTenantIdQueryHandler(IRepository repository, ISender sender, ILogger<GetByTenantIdQueryHandler> logger)
    {
        _repository = repository;
        _sender = sender;
        _logger = logger;
    }

    public async Task<IFluentResults<List<InventoryResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetByTenantId(request.id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<List<InventoryResponse>>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<List<InventoryResponse>>().WithMessage("Inventory Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<List<InventoryResponse>>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<List<InventoryResponse>>().FromResults(result.Result),
            _ => ResultsTo.Something(await GetProductDetails(result.Result!.Value, cancellationToken)),
        };
    }

    private async Task<List<InventoryResponse>> GetProductDetails(IEnumerable<Persistence.Models.Inventory> inventories, CancellationToken cancellationToken = default)
    {
        List<InventoryResponse> result = new();

        foreach (var inventory in inventories)
        {
            var details = await inventory.GetProductDetails(_sender, cancellationToken);
            result.Add(details.Value.BuildInventoryResponse(inventory));
        }

        return result;
    }
}
