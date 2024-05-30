using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Person.Models;

namespace Point.Of.Sale.Person.Handlers.Query.GetByTenantId;

public sealed record GetByTenantIdQuery(int Id) : IQuery<List<PersonResponse>>
{
}