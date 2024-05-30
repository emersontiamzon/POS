using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Inventory.Handlers.Command.Register;

public sealed record RegisterCommand : ICommand
{
    public int TenantId { get; set; }
    public int CategoryId { get; set; }
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public int Quantity { get; set; }
}