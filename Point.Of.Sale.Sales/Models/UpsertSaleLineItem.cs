namespace Point.Of.Sale.Sales.Models;

public class UpsertSaleLineItem
{
    public int SaleId { get; set; }
    public int LineId { get; set; }
    public int TenantId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineDiscount { get; set; }
    public decimal LineTax { get; set; }
    public decimal LineTotal { get; set; }
    public bool Active { get; set; }
}
