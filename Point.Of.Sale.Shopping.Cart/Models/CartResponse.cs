namespace Point.Of.Sale.Shopping.Cart.Models;

public record CartResponse
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ItemCount { get; set; }
    public bool Active { get; set; }
    public int TenantId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}