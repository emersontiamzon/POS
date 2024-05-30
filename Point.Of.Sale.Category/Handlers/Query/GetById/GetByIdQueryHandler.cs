using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Category.Handlers.Query.GetById;

public sealed class GetByIdQueryHandler : IQueryHandler<GetById, CategoryResponse>
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository, ILogger<GetByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<CategoryResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<CategoryResponse>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<CategoryResponse>().WithMessage("Category Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<CategoryResponse>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<CategoryResponse>().FromResults(result.Result),
            _ => ResultsTo.Success(new CategoryResponse
            {
                Id = result.Result.Value.Id,
                Name = result.Result.Value.Name,
                TenantId = result.Result.Value.TenantId,
                Description = result.Result.Value.Description,
                CreatedOn = result.Result.Value.CreatedOn,
                UpdatedOn = result.Result.Value.UpdatedOn,
            }),
        };
    }
}
