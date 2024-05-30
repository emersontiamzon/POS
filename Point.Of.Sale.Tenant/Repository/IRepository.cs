using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Tenant.Repository;

public interface IRepository : IGenericRepository<Persistence.Models.Tenant>
{
    // Task<IFluentResults<List<Persistence.Models.Tenant>>> GetAll(CancellationToken cancellationToken = default);
    // Task<IFluentResults<Persistence.Models.Tenant>> GetById(int request, CancellationToken cancellationToken = default);
    // Task<IFluentResults> Add(UpsertTenant request, CancellationToken cancellationToken = default);
    // Task<IFluentResults> Update(UpsertTenant request, CancellationToken cancellationToken = default);
    Task<IFluentResults<string>> GetApiKeyById(int id, CancellationToken cancellationToken = default);
    Task<IFluentResults<string>> UpdateApiKey(int id, CancellationToken cancellationToken = default);
}
