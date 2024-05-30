namespace Point.Of.Sale.Customer.Models;

public record CustomerResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int TenantId { get; set; }
    public string UpdatedBy { get; set; }
}
