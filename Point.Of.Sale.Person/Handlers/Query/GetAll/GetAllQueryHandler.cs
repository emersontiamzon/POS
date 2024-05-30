using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;
using Point.Of.Sale.Person.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;

namespace Point.Of.Sale.Person.Handlers.Query.GetAll;

public sealed class GetAllQueryHandler : IQueryHandler<GetAllQuery, List<PersonResponse>>
{
    private readonly IRepository _repository;

    public GetAllQueryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IFluentResults<List<PersonResponse>>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAll(cancellationToken);

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

        if (result.IsNotFound() || result.IsFailure())
        {
            return ResultsTo.Failure<List<PersonResponse>>("Person Not Found");
        }

        var response = result.Value.Select(r => new PersonResponse
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
            .ToList();

        return ResultsTo.Success(response);
    }
}