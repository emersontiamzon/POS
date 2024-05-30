using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Tenant.Handlers.Query.GetTenantById;

public sealed record GetTenantById(int Id) : IQuery<TenantResponse>
{
}