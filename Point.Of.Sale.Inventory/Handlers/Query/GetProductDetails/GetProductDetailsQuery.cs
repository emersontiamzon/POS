using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Models;

namespace Point.Of.Sale.Inventory.Handlers.Query.GetProductDetails;

public sealed record GetProductDetailsQuery(int tenandId, int categoryId, int productId, int supplierId) : IQuery<ProductDetailsResponse>
{
}