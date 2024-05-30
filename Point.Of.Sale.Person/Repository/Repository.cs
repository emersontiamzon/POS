using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Person.Repository;

public class Repository : GenericRepository<Persistence.Models.Person>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<CrudResult<Persistence.Models.Person>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        // var person = await _dbContext.Persons.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);
        //
        // if (person is null)
        // {
        //     return ResultsTo.NotFound<CrudResult<Persistence.Models.Person>>($"No Person found with Id {request.EntityId}.");
        // }
        //
        // person.TenantId = request.TenantId;
        //
        // return ResultsTo.Something(new CrudResult<Persistence.Models.Person>
        // {
        //     Count = await _dbContext.SaveChangesAsync(cancellationToken),
        //     Entity = person,
        // });

        return ResultsTo.Failure<CrudResult<Persistence.Models.Person>>();
    }

    public async Task<IFluentResults<List<Persistence.Models.Person>>> GetByTenantId(int id, CancellationToken cancellationToken = default)
    {
        // var result = await _dbContext.Persons.Where(t => t.TenantId == id).ToListAsync(cancellationToken);
        // return ResultsTo.Something(result!);
        return ResultsTo.Failure<List<Persistence.Models.Person>>();
    }
}
