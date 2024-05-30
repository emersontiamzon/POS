using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Tenant.Models;

public class UpsertTenant
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public TenantType Type { get; set; }
    public bool Active { get; set; }
}
