using System.Security.Claims;
using Point.Of.Sale.Shared.Configuration;

namespace Point.Of.Sale.Persistence.Models;

public record TokenBuilderParameters
{
    public List<Claim> Claims { get; set; }
    public PosConfiguration Configuration { get; set; }
    public TimeSpan ExpiresIn { get; set; }
}
