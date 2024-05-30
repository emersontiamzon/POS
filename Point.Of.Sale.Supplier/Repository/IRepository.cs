using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Supplier.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Supplier>
{
    Task<IFluentResults<CrudResult<Persistence.Models.Supplier>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Supplier>>> GetByTenantId(int request, CancellationToken cancellationToken = default);
}
