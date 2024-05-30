namespace Point.Of.Sale.Shared.Configuration;

public record General
{
    public string Environment { get; set; }
    public string ServiceName { get; set; }
    public string ServiceVersion { get; set; }
    public string ServiceDescription { get; set; }
    public string ServiceId { get; set; }
    public string ServicePort { get; set; }
    public string ServiceUrl { get; set; }
    public string ServiceHost { get; set; }
    public string SecretKey { get; set; }
}
