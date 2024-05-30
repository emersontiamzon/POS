using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;
using Polly;

namespace Point.Of.Sale.Tenant.Handlers.Query.GetApiKeyById;

public class GetApiKeyByIdQueryHandler : IQueryHandler<GetApiKeyByIdQuery, string>
{
    private readonly ILogger<GetApiKeyByIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetApiKeyByIdQueryHandler(ILogger<GetApiKeyByIdQueryHandler> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IFluentResults<string>> Handle(GetApiKeyByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetApiKeyById(request.Id, cancellationToken), _logger);

        return result switch
        {
            {Result: null, Outcome: OutcomeType.Failure} => ResultsTo.Failure<string>().FromException(result.FinalException),
            _ => ResultsTo.Something(result.Result!.Value),
        };
    }
}
