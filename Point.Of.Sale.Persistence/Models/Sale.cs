using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Persistence.Models;

public class Sale
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int CustomerId { get; set; }
    public List<SaleLineItem> LineItems { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal TotalDiscounts { get; set; }
    public decimal TaxPercentage { get; set; }
    public decimal SalesTax { get; set; }
    public decimal TotalSales { get; set; }
    public DateTime SaleDate { get; set; }
    public bool Active { get; set; }
    public SaleStatus Status { get; set; }
}
