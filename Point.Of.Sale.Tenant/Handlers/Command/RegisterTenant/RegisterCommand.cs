using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Tenant.Handlers.Command.RegisterTenant;

public sealed record RegisterCommand : ICommand
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public TenantType Type { get; set; }
    public bool Active { get; set; }
}