using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Product.Handlers.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, ProductResponse>
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository, ILogger<GetByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<ProductResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.Id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<ProductResponse>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<ProductResponse>().WithMessage("Product Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<ProductResponse>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<ProductResponse>().FromResults(result.Result),
            _ => ResultsTo.Success(new ProductResponse
            {
                Id = result.Result.Value.Id,
                SkuCode = result.Result.Value.SkuCode,
                Name = result.Result.Value.Name,
                Description = result.Result.Value.Description,
                UnitPrice = result.Result.Value.UnitPrice,
                SupplierId = result.Result.Value.SupplierId,
                CategoryId = result.Result.Value.CategoryId,
                Active = result.Result.Value.Active,
                CreatedOn = result.Result.Value.CreatedOn,
                UpdatedOn = result.Result.Value.UpdatedOn,
                UpdatedBy = result.Result.Value.UpdatedBy,
                TenantId = result.Result.Value.TenantId,
                WebSite = result.Result.Value.WebSite,
                Image = result.Result.Value.Image,
                BarCodeType = result.Result.Value.BarCodeType,
                Barcode = result.Result.Value.Barcode,
            }),
        };
    }
}
