namespace Point.Of.Sale.Customer.Models;

public class UpdateCustomer
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool Active { get; set; }
}
