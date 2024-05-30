using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Models;
using Point.Of.Sale.Tenant.Repository;
using Polly;

namespace Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;

internal sealed class GetTenantByIdQueryHandler : IQueryHandler<GetTenantById, TenantResponse>
{
    private readonly ILogger<GetTenantByIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetTenantByIdQueryHandler(ILogger<GetTenantByIdQueryHandler> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IFluentResults<TenantResponse>> Handle(GetTenantById request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.Id, cancellationToken), _logger);

        return result switch
        {
            {Result: null, Outcome: OutcomeType.Failure} => ResultsTo.Failure<TenantResponse>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<TenantResponse>().WithMessage("Tenant Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<TenantResponse>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<TenantResponse>().FromResults(result.Result),
            _ => ResultsTo.Success(new TenantResponse
            {
                Id = result.Result!.Value.Id,
                Type = result.Result!.Value.Type,
                Code = result.Result!.Value.Code,
                Name = result.Result!.Value.Name,
                Active = result.Result!.Value.Active,
                CreatedDate = result.Result!.Value.CreatedOn.ToLocalTime(),
                UpdatedBy = result.Result!.Value.UpdatedBy,
            }),
        };
    }
}
