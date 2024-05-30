using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;

namespace Point.Of.Sale.Customer.Handlers.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int id) : IQuery<List<CustomerResponse>>
{
}