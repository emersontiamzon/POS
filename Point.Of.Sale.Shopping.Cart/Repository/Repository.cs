using Microsoft.EntityFrameworkCore;
using Point.Of.Sale.Persistence.Context;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Persistence.Repository;
using Point.Of.Sale.Shared.FluentResults;
using Point.Of.Sale.Shared.Models;
using Point.Of.Sale.Shopping.Cart.Models;

namespace Point.Of.Sale.Shopping.Cart.Repository;

public class Repository : GenericRepository<ShoppingCart>, IRepository
{
    private readonly PosDbContext _dbContext;

    public Repository(PosDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IFluentResults<CrudResult<ShoppingCart>>> LinkToTenant(LinkToTenant request, CancellationToken cancellationToken = default)
    {
        var shoppingCart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(t => t.Id == request.EntityId, cancellationToken);

        if (shoppingCart is null)
        {
            return ResultsTo.NotFound<CrudResult<ShoppingCart>>($"No Shopping Cart found with Id {request.EntityId}.");
        }

        shoppingCart.TenantId = request.TenantId;

        return ResultsTo.Something(new CrudResult<ShoppingCart>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = shoppingCart,
        });
    }

    public async Task<IFluentResults<List<ShoppingCart>>> GetByTenantId(int request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.Where(t => t.TenantId == request).ToListAsync(cancellationToken);
        return ResultsTo.Something(result);
    }

    public override async Task<IFluentResults<CrudResult<ShoppingCart>>> Update(ShoppingCart request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (result is null)
        {
            return ResultsTo.NotFound<CrudResult<ShoppingCart>>($"No Shopping Cart found with Id {request.Id}.");
        }

        result.CustomerId = request.CustomerId;
        result.Active = request.Active;
        result.UpdatedOn = DateTime.UtcNow;
        result.UpdatedBy = "User";
        result.ItemCount = result.LineItems.Count;

        return ResultsTo.Something(new CrudResult<ShoppingCart>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = result,
        });
    }

    public async Task<IFluentResults<CrudResult<ShoppingCart>>> UpsertLineItem(UpsertLineItem request, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(t => t.Id == request.CartId, cancellationToken);

        if (result is null)
        {
            return ResultsTo.NotFound<CrudResult<ShoppingCart>>($"No Sale found with Id {request.CartId}.");
        }

        var lineItem = result.LineItems.FirstOrDefault(t => t.LineId == request.LineId);

        if (!result.LineItems.Any() || lineItem is null)
        {
            request.LineId = (result.LineItems.Any() ? result.LineItems.Max(l => l.LineId) : 0) + 1;
            result.LineItems.Add(NewLine(request));

            return ResultsTo.Something(new CrudResult<ShoppingCart>
            {
                Count = await _dbContext.SaveChangesAsync(cancellationToken),
                Entity = result,
            });
        }

        result.LineItems.Remove(lineItem);
        result.LineItems.Add(NewLine(request));
        result.ItemCount = result.LineItems.Count;

        return ResultsTo.Something(new CrudResult<ShoppingCart>
        {
            Count = await _dbContext.SaveChangesAsync(cancellationToken),
            Entity = result,
        });
    }

    private static ShoppingCartLineItem NewLine(UpsertLineItem request)
    {
        return new ShoppingCartLineItem
        {
            LineId = request.LineId,
            TenantId = request.TenantId,
            ProductId = request.ProductId,
            ProductName = request.ProductName,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            ProductDescription = request.ProductDescription,
            LineTotal = request.LineTotal,
        };
    }
}
