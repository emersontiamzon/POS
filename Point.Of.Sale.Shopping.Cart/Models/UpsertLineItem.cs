namespace Point.Of.Sale.Shopping.Cart.Models;

public record UpsertLineItem
{
    public int CartId { get; set; }
    public int LineId { get; set; }
    public int TenantId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}
