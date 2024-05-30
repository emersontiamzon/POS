using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Category.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Category>
{
    Task<IFluentResults<CrudResult<Persistence.Models.Category>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Category>>> GetByTenantId(int id, CancellationToken cancellationToken = default);
}
