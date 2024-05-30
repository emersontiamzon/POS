using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Sales.Models;

namespace Point.Of.Sale.Sales.Handlers.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<SaleResponse>>;