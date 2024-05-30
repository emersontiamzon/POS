using System.ComponentModel;

namespace Point.Of.Sale.Shared.Enums;

public enum TenantType
{
    [Description("NonSpecific")] NonSpecific = 0,
    [Description("Hardware")] Hardware,
    [Description("Pharmacy")] Pharmacy,
    [Description("Grocery")] Grocery,
    [Description("Restaurant")] Restaurant,
    [Description("Liquor")] Liquor,
    [Description("Convenience")] Convenience,
    [Description("Clothing")] Clothing,
    [Description("Auto")] Auto,
    [Description("Furniture")] Furniture,
    [Description("Electronics")] Electronics,
    [Description("Jewelry")] Jewelry,
    [Description("SportingGoods")] SportingGoods,
    [Description("SariSari")] SariSari,
}
