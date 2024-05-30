using System.ComponentModel;

namespace Point.Of.Sale.Shared.Enums;

public enum SaleStatus
{
    [Description("Cancelled")] Cancelled = -1,
    [Description("OrderPlaced")] OrderPlaced = 0,
    [Description("Processed")] Processed,
    [Description("Served")] Served,
    [Description("UnPaid")] UnPaid,
    [Description("Paid")] Paid,
    [Description("Reversed")] Reversed,
}
