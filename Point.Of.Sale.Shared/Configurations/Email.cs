namespace Point.Of.Sale.Shared.Configuration;

public record Smtp
{
    public string Sender { get; set; }
    public string Host { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
}
