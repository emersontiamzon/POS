namespace Point.Of.Sale.Shared.Configuration;

public record Logging
{
    public HoneyComb HoneyComb { get; set; } = new();
}