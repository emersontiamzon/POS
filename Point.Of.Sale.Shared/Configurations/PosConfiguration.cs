namespace Point.Of.Sale.Shared.Configuration;

public class PosConfiguration
{
    public General General { get; set; } = new();
    public Database Database { get; set; } = new();
    public HoneyComb HoneyComb { get; set; } = new();
    public Supabase Supabase { get; set; } = new();
    public Smtp Smtp { get; set; } = new();
    public KeyVault KeyVault { get; set; }
}
