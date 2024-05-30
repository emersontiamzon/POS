using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Customer.Handlers.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, CustomerResponse>
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository, ILogger<GetByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<CustomerResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.Id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<CustomerResponse>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<CustomerResponse>().WithMessage("Tenant Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<CustomerResponse>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<CustomerResponse>().FromResults(result.Result),
            _ => ResultsTo.Success(new CustomerResponse
            {
                Id = result.Result.Value.Id,
                Name = result.Result.Value.Name,
                Address = result.Result.Value.Address,
                PhoneNumber = result.Result.Value.PhoneNumber,
                Email = result.Result.Value.Email,
                CreatedOn = result.Result.Value.CreatedOn,
                UpdatedOn = result.Result.Value.UpdatedOn,
                TenantId = result.Result.Value.TenantId,
            }),
        };
    }
}
