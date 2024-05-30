using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;
using Point.Of.Sale.Product.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Product.Handlers.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<ProductResponse>>
{
    private readonly ILogger<GetAllQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository, ILogger<GetAllQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<List<ProductResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetAll(cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<List<ProductResponse>>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<List<ProductResponse>>().WithMessage("Product Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<List<ProductResponse>>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<List<ProductResponse>>().FromResults(result.Result),
            _ => ResultsTo.Success(result.Result.Value.Select(r => new ProductResponse
                {
                    Id = r.Id,
                    SkuCode = r.SkuCode,
                    Name = r.Name,
                    Description = r.Description,
                    UnitPrice = r.UnitPrice,
                    SupplierId = r.SupplierId,
                    CategoryId = r.CategoryId,
                    Active = r.Active,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    UpdatedBy = r.UpdatedBy,
                    TenantId = r.TenantId,
                    WebSite = r.WebSite,
                    Image = r.Image,
                    BarCodeType = r.BarCodeType,
                    Barcode = r.Barcode,
                })
                .ToList()),
        };
    }
}
