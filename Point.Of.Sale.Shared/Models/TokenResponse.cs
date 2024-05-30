namespace Point.Of.Sale.Shared.Models;

public record TokenResponse
{
    public string BearerToken { get; set; }
    public string RefreshToken { get; set; }
    public string ApiKey { get; set; }
    public string UserName { get; set; }
}
