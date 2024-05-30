namespace Point.Of.Sale.Persistence.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string EntityName { get; set; }
    public string EntityId { get; set; }
    public string Action { get; set; }
    public string Changes { get; set; }
    public DateTime CreatedOn { get; set; }
    public string UpdatedBy { get; set; }
}
