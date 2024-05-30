namespace Point.Of.Sale.Category.Models;

public class AddCategory
{
    public int TenantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
}
