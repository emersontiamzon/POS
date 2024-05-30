using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Models;
using Point.Of.Sale.Tenant.Repository;
using Polly;

namespace Point.Of.Sale.Tenant.Handlers.Query.GetAllTenants;

internal sealed class GetAllQueryHandler : IQueryHandler<GetAll, List<TenantResponse>>
{
    private readonly ILogger<GetAllQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetAllQueryHandler(ILogger<GetAllQueryHandler> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IFluentResults<List<TenantResponse>>> Handle(GetAll request, CancellationToken cancellationToken)
    {
        var aa = await _repository.GetAll(cancellationToken);
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetAll(cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<List<TenantResponse>>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<List<TenantResponse>>().WithMessage("Tenant Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<List<TenantResponse>>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<List<TenantResponse>>().FromResults(result.Result),
            _ => ResultsTo.Success(result.Result!.Value.Select(r => new TenantResponse
                {
                    Id = r.Id,
                    Type = r.Type,
                    Code = r.Code,
                    Name = r.Name,
                    Active = r.Active,
                    CreatedDate = r.CreatedOn.ToLocalTime(),
                    UpdatedBy = r.UpdatedBy,
                })
                .ToList()),
        };
    }
}
