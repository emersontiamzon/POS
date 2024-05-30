using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Category.Models;

namespace Point.Of.Sale.Category.Handlers.Query.GetAll;

public sealed record GetAllCategoriesQuery : IQuery<List<CategoryResponse>>
{
}