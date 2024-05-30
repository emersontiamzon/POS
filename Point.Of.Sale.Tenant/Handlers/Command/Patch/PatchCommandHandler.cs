using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;
using Polly;

namespace Point.Of.Sale.Tenant.Handlers.Command.Patch;

public sealed class PatchCommandHandler : ICommandHandler<PatchCommand>
{
    private readonly ILogger<PatchCommandHandler> _logger;
    private readonly IRepository _repository;

    public PatchCommandHandler(ILogger<PatchCommandHandler> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(PatchCommand request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Patch(request.Id, request.Patch, cancellationToken), _logger);

        return result switch
        {
            {Result: null, Outcome: OutcomeType.Failure} => ResultsTo.Failure<string>().FromException(result.FinalException),
            _ => ResultsTo.Something(result.Result!.Value),
        };
    }
}
