using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Customer.Handlers.Command.Update;

public sealed record UpdateCommand : ICommand
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool Active { get; set; }
}