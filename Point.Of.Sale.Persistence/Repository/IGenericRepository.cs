using Microsoft.AspNetCore.JsonPatch;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Persistence.Repository;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<IFluentResults<IEnumerable<TEntity>>> GetAll(CancellationToken cancellationToken = default);
    Task<IFluentResults<TEntity>> GetById(object id, CancellationToken cancellationToken = default);
    Task<IFluentResults<CrudResult<TEntity>>> Add(TEntity obj, CancellationToken cancellationToken = default);
    Task<IFluentResults<CrudResult<TEntity>>> Update(TEntity obj, CancellationToken cancellationToken = default);
    Task<IFluentResults<CrudResult<TEntity>>> Delete(object id, CancellationToken cancellationToken = default);
    Task<IFluentResults<CrudResult<TEntity>>> Delete(object id, string activeEntityField, CancellationToken cancellationToken = default);
    Task<IFluentResults<CrudResult<TEntity>>> Patch(object id, JsonPatchDocument<TEntity> patch, CancellationToken cancellationToken = default);
}
