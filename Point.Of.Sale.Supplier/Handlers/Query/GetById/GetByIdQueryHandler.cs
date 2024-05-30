using Microsoft.Extensions.Logging;
using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Retries.RetryPolicies;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Supplier.Models;
using Point.Of.Sale.Supplier.Repository;
using Polly;

namespace Point.Of.Sale.Supplier.Handlers.Query.GetById;

internal sealed class GetByIdQueryHandler : IQueryHandler<GetById, SupplierResponse>
{
    private readonly ILogger<GetByIdQueryHandler> _logger;
    private readonly IRepository _repository;

    public GetByIdQueryHandler(IRepository repository, ILogger<GetByIdQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IFluentResults<SupplierResponse>> Handle(GetById request, CancellationToken cancellationToken)
    {
        var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetById(request.Id, cancellationToken), _logger);

        return result switch
        {
            {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<SupplierResponse>().FromException(result.FinalException),
            {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<SupplierResponse>().WithMessage("Supplier Not Found"),
            {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<SupplierResponse>().WithMessage("Bad Request"),
            {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<SupplierResponse>().FromResults(result.Result),
            _ => ResultsTo.Success(new SupplierResponse
            {
                Id = result.Result!.Value.Id,
                Name = result.Result.Value.Name,
                Address = result.Result.Value.Address,
                Phone = result.Result.Value.Phone,
                Email = result.Result.Value.Email,
                City = result.Result.Value.City,
                State = result.Result.Value.State,
                Country = result.Result.Value.Country,
                Active = result.Result.Value.Active,
                CreatedOn = result.Result.Value.CreatedOn.ToLocalTime(),
                UpdatedOn = result.Result.Value.UpdatedOn.ToLocalTime(),
                TenantId = result.Result.Value.TenantId,
            }),
        };
    }
}
