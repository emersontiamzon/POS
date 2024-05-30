using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Customer.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Customer>
{
    Task<IFluentResults<CrudResult<Persistence.Models.Customer>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Customer>>> GetByTenantId(int id, CancellationToken cancellationToken = default);
}
