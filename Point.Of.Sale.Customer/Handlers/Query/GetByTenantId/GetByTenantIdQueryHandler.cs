using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;
using Point.Of.Sale.Customer.Repository;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Polly;

namespace Point.Of.Sale.Customer.Handlers.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<CustomerResponse>>
{
    private readonly ILogger<GetByTenantIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository, ILogger<GetByTenantIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<List<CustomerResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetByTenantId(request.id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<List<CustomerResponse>>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<List<CustomerResponse>>().WithMessage("Customer Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<List<CustomerResponse>>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<List<CustomerResponse>>().FromResults(result.Result),
            _ => ResultsTo.Success(result.Result!.Value.Select(r => new CustomerResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    PhoneNumber = r.PhoneNumber,
                    Email = r.Email,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    TenantId = r.TenantId,
                })
                .ToList()),
        };
    }
}
