using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Product.Models;

namespace Point.Of.Sale.Product.Handlers.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<ProductResponse>>
{
}