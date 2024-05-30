using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Customer.Repository;

public class Repository : GenericRepository<Persistence.Models.Customer>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<CrudResult<Persistence.Models.Customer>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var customer = await _dbContext.Customers.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (customer is null)
        {
            return ResultsTo.NotFound<CrudResult<Persistence.Models.Customer>>($"No Customer found with Id {request.EntityId}.");
        }

        customer.TenantId = request.TenantId;

        return ResultsTo.Something(new CrudResult<Persistence.Models.Customer>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = customer,
        });
    }

    public async Task<IFluentResults<List<Persistence.Models.Customer>>> GetByTenantId(int id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Customers.Where(t => t.TenantId == id).ToListAsync(cancellationToken);
        return ResultsTo.Something(result);
    }
}
