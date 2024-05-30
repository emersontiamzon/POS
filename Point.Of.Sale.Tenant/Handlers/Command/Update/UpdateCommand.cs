using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Tenant.Handlers.Command.Update;

public sealed record UpdateCommand : ICommand
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public TenantType Type { get; set; }
    public bool Active { get; set; }
}