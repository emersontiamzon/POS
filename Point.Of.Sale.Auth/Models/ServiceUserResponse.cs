namespace Point.Of.Sale.Auth.Models;

public record ServiceUserResponse
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int TenantId { get; set; }
}