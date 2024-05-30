namespace Point.Of.Sale.Shared.Configuration;

public record Database
{
    public string Server { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public int Port { get; set; }
    public DbProvider DbProvider { get; set; }
}
