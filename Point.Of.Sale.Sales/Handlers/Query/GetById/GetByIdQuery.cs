using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Models;

namespace Point.Of.Sale.Sales.Handlers.Query.GetById;

public sealed record GetById(int Id) : IQuery<SaleResponse>;