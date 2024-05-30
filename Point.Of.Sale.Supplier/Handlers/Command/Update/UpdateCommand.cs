using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Supplier.Handlers.Command.Update;

public sealed record UpdateCommand : ICommand
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }
}