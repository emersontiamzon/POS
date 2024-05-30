using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Shopping.Cart.Handlers.Command.Update;

public sealed record UpdateCommand : ICommand
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public int CustomerId { get; set; }
    public bool Active { get; set; }
}
