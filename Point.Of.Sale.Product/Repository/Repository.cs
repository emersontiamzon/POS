using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Product.Repository;

public class Repository : GenericRepository<Persistence.Models.Product>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<CrudResult<Persistence.Models.Product>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (product is null)
        {
            return ResultsTo.NotFound<CrudResult<Persistence.Models.Product>>($"No Product found with Id {request.EntityId}.");
        }

        product.TenantId = request.TenantId;

        return ResultsTo.Something(new CrudResult<Persistence.Models.Product>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = product,
        });
    }


    public async Task<IFluentResults<List<Persistence.Models.Product>>> GetByTenantId(int id, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Products.Where(t => t.TenantId == id).ToListAsync(cancellationToken);
        return ResultsTo.Something(result);
    }
}
