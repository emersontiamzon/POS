namespace Point.Of.Sale.Shopping.Cart.Models;

public class UpsertCart
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int CustomerId { get; set; }
    public bool Active { get; set; }
}
