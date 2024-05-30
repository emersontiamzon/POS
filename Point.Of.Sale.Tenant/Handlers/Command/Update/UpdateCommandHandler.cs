using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Tenant.Repository;
using Polly;

namespace Point.Of.Sale.Tenant.Handlers.Command.Update;

public class UpdateCommandHandler : ICommandHandler<UpdateCommand>
{
    private readonly ILogger<UpdateCommandHandler> _logger;
    private readonly IRepository _repository;

    public UpdateCommandHandler(IRepository repository, ILogger<UpdateCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.Update(new Persistence.Models.Tenant
        {
            Id = request.Id,
            Type = request.Type,
            Code = request.Code,
            Name = request.Name,
            Active = request.Active,
            UpdatedOn = DateTime.UtcNow,
            UpdatedBy = "User",
        }, cancellationToken), _logger);

        return result switch
        {
            {Result: null, Outcome: OutcomeType.Failure} => ResultsTo.Failure().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound("Tenant Not Found"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Something(result.Result),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.Something(result.Result),
            _ => ResultsTo.Something(result.Result!),
        };
    }
}
