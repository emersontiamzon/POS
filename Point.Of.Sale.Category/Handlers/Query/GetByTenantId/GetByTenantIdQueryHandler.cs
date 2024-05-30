using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;
using Point.Of.Sale.Category.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Category.Handlers.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<CategoryResponse>>
{
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<CategoryResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var a = await _repository.GetByTenantId(request.id, cancellationToken);
        return ResultsTo.NotFound<List<CategoryResponse>>().WithMessage("Category Not Found");
        // var result = await PosPolicies.ExecuteThenCaptureResult(() => _repository.GetByTenantId(request.id, cancellationToken));
        //
        // return result switch
        // {
        //     {Result: null} or {Outcome: OutcomeType.Failure} => ResultsTo.Failure<List<CategoryResponse>>().FromException(result.FinalException),
        //     {Result.Status: FluentResultsStatus.NotFound} => ResultsTo.NotFound<List<CategoryResponse>>().WithMessage("Category Not Found"),
        //     {Result.Status: FluentResultsStatus.BadRequest} => ResultsTo.BadRequest<List<CategoryResponse>>().WithMessage("Bad Request"),
        //     {Result.Status: FluentResultsStatus.Failure} => ResultsTo.Failure<List<CategoryResponse>>().FromResults(result.Result),
        //     _ => ResultsTo.Success(result.Result!.Value.Select(r => new CategoryResponse
        //         {
        //             Id = r.Id,
        //             Name = r.Name,
        //             TenantId = r.TenantId,
        //             Description = r.Description,
        //             CreatedOn = r.CreatedOn,
        //             UpdatedOn = r.UpdatedOn,
        //         })
        //         .ToList()),
        // };
    }
}
