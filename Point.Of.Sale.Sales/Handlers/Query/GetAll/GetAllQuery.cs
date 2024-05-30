using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Models;

namespace Point.Of.Sale.Sales.Handlers.Query.GetAll;

public sealed record GetAllQuery : IQuery<List<SaleResponse>>;