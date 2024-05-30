using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Sales.Models;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Sales.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Sale>
{
    Task<IFluentResults<CrudResult<Persistence.Models.Sale>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<List<Persistence.Models.Sale>>> GetByTenantId(int id, CancellationToken cancellationToken = default);
    Task<IFluentResults<CrudResult<Persistence.Models.Sale>>> UpsertLineItem(UpsertSaleLineItem request, CancellationToken cancellationToken = default);
}
