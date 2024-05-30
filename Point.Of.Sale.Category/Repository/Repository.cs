using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Category.Repository;

public class Repository : GenericRepository<Persistence.Models.Category>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<CrudResult<Persistence.Models.Category>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (category is null)
        {
            return ResultsTo.NotFound<CrudResult<Persistence.Models.Category>>($"No Category found with Id {request.EntityId}.");
        }

        category.TenantId = request.TenantId;

        return ResultsTo.Something(new CrudResult<Persistence.Models.Category>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = category,
        });
    }

    public async Task<IFluentResults<List<Persistence.Models.Category>>> GetByTenantId(int id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Categories.Where(t => t.TenantId == id).ToListAsync(cancellationToken);
        return ResultsTo.Something(result);
    }
}
