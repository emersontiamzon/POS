using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Tenant.Handlers.Query.GetApiKeyById;

public sealed record GetApiKeyByIdQuery(int Id) : IQuery<string>
{
}