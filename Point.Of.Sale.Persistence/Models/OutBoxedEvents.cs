using Point.Of.Sale.Shared.Enums;

namespace Point.Of.Sale.Persistence.Models;

public class OutBoxedEvents
{
    public int Id { get; set; }
    public DomainEvents DomainEvent { get; set; }
    public int TotalTask { get; set; }
    public int CompletedTask { get; set; }
    public OutBoxEventStatus Status { get; set; }
    public DateTime PublishedOn { get; set; }
    public DateTime? ScheduledOn { get; set; }
    public DateTime? StartedOn { get; set; }
    public DateTime? FinalizedOn { get; set; }
    public string Payload { get; set; }
    public List<string> BatchErrors { get; set; }
}
