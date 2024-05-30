using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Command.Register;

public sealed record RegisterCommand : ICommand
{
    public int TenantId { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int ItemCount { get; set; }
    public bool Active { get; set; }
}