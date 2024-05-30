using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Inventory.Handlers.Command.Update;

public sealed record UpdateCommand : ICommand
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int CategoryId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public bool Active { get; set; }
}