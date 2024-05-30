using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Customer.Handlers.Command.Register;

public sealed record RegisterCommand : ICommand
{
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}