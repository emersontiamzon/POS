using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;

namespace Point.Of.Sale.Category.Handlers.Query.GetById;

public sealed record GetById(int id) : IQuery<CategoryResponse>
{
}