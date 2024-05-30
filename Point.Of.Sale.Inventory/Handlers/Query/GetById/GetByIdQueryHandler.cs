using MediatR;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Handlers.Query.Extension;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Inventory.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Inventory.Handlers.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, InventoryResponse>
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IRepository _repository;
    private readonly ISender _sender;

    public GetByIdQueryHandler(IRepository repository, ISender sender, ILogger<GetByIdQueryHandler> logger)
    {
        _repository = repository;
        _sender = sender;
        _logger = logger;
    }

    public async Task<IFluentResults<InventoryResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.Id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<InventoryResponse>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<InventoryResponse>().WithMessage("Inventory Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<InventoryResponse>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<InventoryResponse>().FromResults(result.Result),
            _ => ResultsTo.Something(await GetProductDetails(result.Result!.Value, cancellationToken)),
        };
    }

    private async Task<InventoryResponse> GetProductDetails(Persistence.Models.Inventory inventory, CancellationToken cancellationToken = default)
    {
        var details = await inventory.GetProductDetails(_sender, cancellationToken);
        return details.Value.BuildInventoryResponse(inventory);
    }
}