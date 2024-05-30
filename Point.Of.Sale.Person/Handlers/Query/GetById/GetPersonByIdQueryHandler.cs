using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;
using Point.Of.Sale.Person.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Person.Handlers.Query.GetById;

internal sealed class GetPersonByIdQueryHandler : IQueryHandler<GetPersonById, PersonResponse>
{
    private readonly IRepository _repository;

    public GetPersonByIdQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<PersonResponse>> Handle(GetPersonById request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetById(request.Id, cancellationToken);

        return result.Status switch
        {
            FluentResultsStatus.NotFound => ResultsTo.NotFound<PersonResponse>().WithMessage("Person Not Found"),
            FluentResultsStatus.BadRequest => ResultsTo.BadRequest<PersonResponse>().WithMessage("Bad Request"),
            FluentResultsStatus.Failure => ResultsTo.Failure<PersonResponse>().FromResults(result),
            _ => ResultsTo.Success(new PersonResponse
            {
                Id = result.Value.Id,
                FirstName = result.Value.FirstName,
                MiddleName = result.Value.MiddleName,
                LastName = result.Value.LastName,
                Suffix = result.Value.Suffix,
                Genmder = result.Value.Gender,
                BirthDate = result.Value.BirthDate,
                Address = result.Value.Address,
                Email = result.Value.Email,
                CreatedOn = result.Value.CreatedOn,
                UpdatedOn = result.Value.UpdatedOn,
                TenantId = result.Value.TenantId,
            }),
        };
    }
}