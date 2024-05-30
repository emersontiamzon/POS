namespace Point.Of.Sale.Shared.Models;

public record LinkToTenant
{
    public int EntityId { get; set; } //Id of the entity to be linked to a tenant
    public int TenantId { get; set; } //link to this tenant
}
