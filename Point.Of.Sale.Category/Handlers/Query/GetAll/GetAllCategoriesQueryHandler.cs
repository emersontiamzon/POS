using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Category.Handlers.Query.GetAll;

public sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<CategoryResponse>>
{
    private readonly ILogger<GetAllCategoriesQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetAllCategoriesQueryHandler(IRepository repository, ILogger<GetAllCategoriesQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<List<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetAll(cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<List<CategoryResponse>>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<List<CategoryResponse>>().WithMessage("Category Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<List<CategoryResponse>>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<List<CategoryResponse>>().FromResults(result.Result),
            _ => ResultsTo.Success(result.Result!.Value.Select(r => new CategoryResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    TenantId = r.TenantId,
                    Description = r.Description,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                })
                .ToList()),
        };
    }
}
