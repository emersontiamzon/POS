namespace Point.Of.Sale.Auth.Models;

public class ValidateUserRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int TenantId { get; set; }
}