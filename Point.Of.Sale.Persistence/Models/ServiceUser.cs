using Microsoft.AspNetCore.Identity;

namespace Point.Of.Sale.Persistence.Models;

public class ServiceUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string RefreshToken { get; set; }
    public string ApiToken { get; set; }
    public int TenantId { get; set; }
    public bool Active { get; set; }
}
