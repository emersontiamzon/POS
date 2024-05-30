using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.FluentResults.Extension;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Persistence.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly PosDbContext _dbContext;
    private readonly DbSet<TEntity> _entity;

    protected GenericRepository(PosDbContext dbContext)
    {
        _dbContext = dbContext;
        _entity = dbContext.Set<TEntity>();
    }

    public virtual async Task<IFluentResults<TEntity>> GetById(object id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _entity.FindAsync(id, cancellationToken);

            if (result == null)
            {
                return ResultsTo.NotFound<TEntity>();
            }

            return ResultsTo.Something(result);
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<TEntity>().FromException(e);
        }
    }

    public virtual async Task<IFluentResults<IEnumerable<TEntity>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _entity.ToListAsync(cancellationToken);
        return ResultsTo.Something<IEnumerable<TEntity>>(result);
    }

    public virtual async Task<IFluentResults<CrudResult<TEntity>>> Update(TEntity obj, CancellationToken cancellationToken = default)
    {
        try
        {
            _entity.Update(obj);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return ResultsTo.Something(new CrudResult<TEntity> {Count = result, Entity = obj});
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<CrudResult<TEntity>>().FromException(e);
        }
    }

    public virtual async Task<IFluentResults<CrudResult<TEntity>>> Add(TEntity obj, CancellationToken cancellationToken = default)
    {
        try
        {
            await _entity.AddAsync(obj);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            return ResultsTo.Something(new CrudResult<TEntity> {Count = result, Entity = obj});
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<CrudResult<TEntity>>().FromException(e);
        }
    }

    public virtual async Task<IFluentResults<CrudResult<TEntity>>> Delete(object id, CancellationToken cancellationToken = default)
    {
        try
        {
            var forDelete = await GetById(id, cancellationToken);

            if (forDelete.IsNotFoundOrBadRequest())
            {
                return ResultsTo.NotFound(new CrudResult<TEntity> {Count = 0, Entity = null});
            }

            _entity.Remove(forDelete.Value!);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return ResultsTo.Something(new CrudResult<TEntity> {Count = result, Entity = forDelete.Value!});
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<CrudResult<TEntity>>().FromException(e);
        }
    }

    public virtual async Task<IFluentResults<CrudResult<TEntity>>> Delete(object id, string activeEntityField, CancellationToken cancellationToken = default)
    {
        try
        {
            var forDelete = await GetById(id, cancellationToken);

            if (forDelete.IsNotFoundOrBadRequest())
            {
                return ResultsTo.NotFound(new CrudResult<TEntity> {Count = 0, Entity = null});
            }

            forDelete.Value!.GetType().GetProperty(activeEntityField)?.SetValue(forDelete.Value, false);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);
            return ResultsTo.Something(new CrudResult<TEntity> {Count = result, Entity = forDelete.Value!});
        }
        catch (Exception e)
        {
            return ResultsTo.Failure<CrudResult<TEntity>>().FromException(e);
        }
    }

    public virtual async Task<IFluentResults<CrudResult<TEntity>>> Patch(object id, JsonPatchDocument<TEntity> patch, CancellationToken cancellationToken = default)
    {
        var forPatching = await GetById(id, cancellationToken);

        if (forPatching.IsNotFoundOrBadRequest())
        {
            return ResultsTo.NotFound(new CrudResult<TEntity> {Count = 0, Entity = null});
        }

        patch.ApplyTo(forPatching.Value);
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return ResultsTo.Something(new CrudResult<TEntity> {Count = result, Entity = forPatching.Value!});
    }
}
