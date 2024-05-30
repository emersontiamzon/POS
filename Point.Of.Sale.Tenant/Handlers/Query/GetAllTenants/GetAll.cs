using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Tenant.Models;

namespace Point.Of.Sale.Tenant.Handlers.Query.GetAllTenants;

public sealed record GetAll : IQuery<List<TenantResponse>>
{
}