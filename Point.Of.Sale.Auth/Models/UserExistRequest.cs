namespace Point.Of.Sale.Auth.Models;

public class UserExistRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public int TenantId { get; set; }
}