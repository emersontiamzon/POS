using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Product.Models;

public record ProductResponse
{
    public int Id { get; set; }
    public string SkuCode { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public int SupplierId { get; set; }
    public int CategoryId { get; set; }
    public int TenantId { get; set; }
    public string WebSite { get; set; }
    public byte[] Image { get; set; }
    public BarCodes BarCodeType { get; set; }
    public byte[] Barcode { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public bool Active { get; set; }
}