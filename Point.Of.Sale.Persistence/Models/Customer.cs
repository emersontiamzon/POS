namespace Point.Of.Sale.Persistence.Models;

public class Customer
{
    public int Id { get; set; }
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string UpdatedBy { get; set; }
    public bool Active { get; set; }
}
