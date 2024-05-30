using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Inventory.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Inventory>
{
    Task<IFluentResults<CrudResult<Persistence.Models.Inventory>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Inventory>>> GetByTenantId(int id, CancellationToken cancellationToken = default);
}
