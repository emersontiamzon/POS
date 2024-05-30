using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shopping.Cart.Models;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<CartResponse>>
{
}