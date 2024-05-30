namespace Point.Of.Sale.Shared.Configuration;

public record Supabase
{
    public string Url { get; set; }
    public string ApiKey { get; set; }
}
