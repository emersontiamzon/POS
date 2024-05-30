using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Tenant.Models;

public record TenantResponse
{
    public int Id { get; set; }
    public TenantType Type { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UpdatedBy { get; set; }
}
