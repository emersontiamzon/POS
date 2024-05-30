namespace Point.Of.Sale.Events.Domain;

public interface IControllerLogEvent
{
    string User { get; set; }
    DateTime ActionOn { get; set; }
    List<Dictionary<string, object?>> Data { get; set; }
}
