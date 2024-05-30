using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Inventory.Repository;

public class Repository : GenericRepository<Persistence.Models.Inventory>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<CrudResult<Persistence.Models.Inventory>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var inventory = await _dbContext.Inventories.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (inventory is null)
        {
            return ResultsTo.NotFound<CrudResult<Persistence.Models.Inventory>>($"No Inventory found with Id {request.EntityId}.");
        }

        inventory.TenantId = request.TenantId;

        return ResultsTo.Something(new CrudResult<Persistence.Models.Inventory>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = inventory,
        });
    }

    public async Task<IFluentResults<List<Persistence.Models.Inventory>>> GetByTenantId(int id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Inventories.Where(t => t.TenantId == id).ToListAsync(cancellationToken);
        return ResultsTo.Something(result!);
    }
}
