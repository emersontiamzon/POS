using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;
using Point.Of.Sale.Person.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Person.Handlers.Query.GetByTenantId;

public sealed class GetByTenantIdQueryHandler : IQueryHandler<GetByTenantIdQuery, List<PersonResponse>>
{
    private readonly IRepository _repository;

    public GetByTenantIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<PersonResponse>>> Handle(GetByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByTenantId(request.Id, cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<List<PersonResponse>>().WithMessage("Person Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<List<PersonResponse>>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<List<PersonResponse>>().FromResults(result),
            _ => ResultsTo.Success(result.Value.Select(r => new PersonResponse
                {
                    Id = r.Id,
                    FirstName = r.FirstName,
                    MiddleName = r.MiddleName,
                    LastName = r.LastName,
                    Suffix = r.Suffix,
                    Genmder = r.Gender,
                    BirthDate = r.BirthDate,
                    Address = r.Address,
                    Email = r.Email,
                    CreatedOn = r.CreatedOn,
                    UpdatedOn = r.UpdatedOn,
                    TenantId = r.TenantId,
                })
                .ToList()),
        };
    }
}