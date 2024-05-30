using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Repository;

namespace Point.Of.Sale.Events.Repository;

public interface IRepository : IGenericRepository<AuditLog>
{
    // Task<IFluentResults<List<Persistence.Models.Tenant>>> GetAll(CancellationToken cancellationToken = default);
    // Task<IFluentResults<Persistence.Models.Tenant>> GetById(int request, CancellationToken cancellationToken = default);
    // Task<IFluentResults> Add(UpsertTenant request, CancellationToken cancellationToken = default);
    // Task<IFluentResults> Update(UpsertTenant request, CancellationToken cancellationToken = default);
    // Task<IFluentResults<string>> GetApiKeyById(int id, CancellationToken cancellationToken = default);}
}
