using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Sales.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Sales.Handlers.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, SaleResponse>
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository, ILogger<GetByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<SaleResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.Id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<SaleResponse>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<SaleResponse>().WithMessage("Sale Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<SaleResponse>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<SaleResponse>().FromResults(result.Result),
            _ => ResultsTo.Success(new SaleResponse
            {
                Id = result.Result.Value.Id,
                CustomerId = result.Result.Value.CustomerId,
                LineItems = result.Result.Value.LineItems,
                Active = result.Result.Value.Active,
                SubTotal = result.Result.Value.SubTotal,
                TotalDiscounts = result.Result.Value.TotalDiscounts,
                TaxPercentage = result.Result.Value.TaxPercentage,
                SalesTax = result.Result.Value.SalesTax,
                TotalSales = result.Result.Value.TotalSales,
                SaleDate = result.Result.Value.SaleDate,
                Status = result.Result.Value.Status,
                TenantId = result.Result.Value.TenantId,
            }),
        };
    }
}
