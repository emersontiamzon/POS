namespace Point.Of.Sale.Inventory.Models;

public class UpsertInventory
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int CategoryId { get; set; }
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public int Quantity { get; set; }
    public bool Active { get; set; }
}
