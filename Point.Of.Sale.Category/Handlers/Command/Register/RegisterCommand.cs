using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Category.Handlers.Command.Register;

public sealed record RegisterCommand : ICommand
{
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
}