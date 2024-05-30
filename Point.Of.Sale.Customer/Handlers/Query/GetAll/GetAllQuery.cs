using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Customer.Models;

namespace Point.Of.Sale.Customer.Handlers.Query.GetAll;

public sealed record GetAllQuery : IQuery<List<CustomerResponse>>
{
}