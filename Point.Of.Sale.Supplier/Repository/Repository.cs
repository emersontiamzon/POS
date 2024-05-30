using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Supplier.Repository;

public class Repository : GenericRepository<Persistence.Models.Supplier>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<List<Persistence.Models.Supplier>>> GetByTenantId(int request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Suppliers.Where(t => t.TenantId == request).ToListAsync(cancellationToken);

        return ResultsTo.Something(result!);
    }

    public async Task<IFluentResults<CrudResult<Persistence.Models.Supplier>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var supplier = await _dbContext.Suppliers.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (supplier is null)
        {
            return ResultsTo.NotFound<CrudResult<Persistence.Models.Supplier>>($"No Supplier found with Id {request.EntityId}.");
        }

        supplier.TenantId = request.TenantId;

        return ResultsTo.Something(new CrudResult<Persistence.Models.Supplier>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = supplier,
        });
    }
}
