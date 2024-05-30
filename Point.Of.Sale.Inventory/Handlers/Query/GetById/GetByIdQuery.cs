using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Inventory.Models;

namespace Point.Of.Sale.Inventory.Handlers.Query.GetById;

public sealed record GetById(int Id) : IQuery<InventoryResponse>
{
}