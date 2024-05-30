using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Sales.Handlers.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<SaleResponse>>
{
    private readonly ILogger<GetByTenantIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository, ILogger<GetByTenantIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<List<SaleResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetByTenantId(request.id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<List<SaleResponse>>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<List<SaleResponse>>().WithMessage("Sale Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<List<SaleResponse>>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<List<SaleResponse>>().FromResults(result.Result),
            _ => ResultsTo.Success(result.Result.Value.Select(r => new SaleResponse
                {
                    Id = r.Id,
                    CustomerId = r.CustomerId,
                    LineItems = r.LineItems,
                    Active = r.Active,
                    SubTotal = r.SubTotal,
                    TotalDiscounts = r.TotalDiscounts,
                    TaxPercentage = r.TaxPercentage,
                    SalesTax = r.SalesTax,
                    TotalSales = r.TotalSales,
                    SaleDate = r.SaleDate,
                    Status = r.Status,
                    TenantId = r.TenantId,
                })
                .ToList()),
        };
    }
}
