namespace Point.Of.Sale.Persistence.Models;

public class Supplier
{
    public int Id { get; set; } //primary key
    public int TenantId { get; set; }
    public string Name { get; set; } //name of the supplier
    public string Address { get; set; } //address of the supplier
    public string City { get; set; } //city of the supplier
    public string State { get; set; } //state of the supplier
    public string Country { get; set; } //country of the supplier
    public string Phone { get; set; } //phone number of the supplier
    public string Email { get; set; } //email of the supplier
    public DateTime CreatedOn { get; set; } //date the supplier was created
    public DateTime UpdatedOn { get; set; } //date the supplier was updated
    public string UpdatedBy { get; set; } //user that updated the supplier
    public bool Active { get; set; }
}
