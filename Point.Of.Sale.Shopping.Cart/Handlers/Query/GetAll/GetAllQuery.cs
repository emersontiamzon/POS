using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shopping.Cart.Models;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Query.GetAll;

public sealed record GetAllQuery : IQuery<List<CartResponse>>
{
}