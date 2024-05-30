using MediatR;
using Point.Of.Sale.Inventory.Handlers.Query.GetProductDetails;
using Point.Of.Sale.Inventory.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Inventory.Handlers.Query.Extension;

public static class QueryExtension
{
    public static async Task<IFluentResults<ProductDetailsResponse>> GetProductDetails(this Persistence.Models.Inventory inventory, ISender sender, CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(new GetProductDetailsQuery(
            inventory.TenantId,
            inventory.CategoryId,
            inventory.ProductId,
            inventory.SupplierId), cancellationToken);

        return ResultsTo.Something(result);
    }

    public static InventoryResponse BuildInventoryResponse(this ProductDetailsResponse details, Persistence.Models.Inventory inventory)
    {
        var result = new InventoryResponse
        {
            Id = inventory.Id,
            TenantId = inventory.TenantId,
            CategoryId = inventory.CategoryId,
            ProductId = inventory.ProductId,
            SupplierId = inventory.SupplierId,
            TenantName = details?.TenantResponse?.Value?.Name ?? string.Empty,
            CategoryName = details?.CategoryResponse?.Value?.Name ?? string.Empty,
            ProductName = details?.ProductResponse?.Value?.Name ?? string.Empty,
            SupplierName = details?.SupplierResponse?.Value?.Name ?? string.Empty,
            Quantity = inventory.Quantity,
            CreatedOn = inventory.CreatedOn.ToLocalTime(),
            UpdatedOn = inventory.UpdatedOn.ToLocalTime(),
            UpdatedBy = inventory.UpdatedBy,
        };

        return result;
    }
}
