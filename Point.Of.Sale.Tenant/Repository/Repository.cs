using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Tenant.Repository;

public class Repository : GenericRepository<Persistence.Models.Tenant>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<string>> GetApiKeyById(int id, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (tenant is null)
        {
            return ResultsTo.NotFound<string>("Tenant not found");
        }

        if (string.IsNullOrWhiteSpace(tenant.TenantApiKey))
        {
            return ResultsTo.NotFound<string>("Api not found");
        }

        return ResultsTo.Something(tenant.TenantApiKey);
    }

    public async Task<IFluentResults<string>> UpdateApiKey(int id, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (tenant is null)
        {
            return ResultsTo.NotFound<string>("Tenant not found");
        }

        return ResultsTo.Something(tenant.TenantApiKey);
    }
}
