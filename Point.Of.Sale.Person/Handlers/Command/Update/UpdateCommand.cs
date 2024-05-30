using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Shared.Enums;
using Point.Of.Sale.Shared.Models;

namespace Point.Of.Sale.Person.Handlers.Command.Update;

public sealed record UpdateCommand : ICommand
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Suffix { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public bool IsUser { get; set; }
    public UserDetails UserDetails { get; set; }
    public bool Active { get; set; }
}