using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Handlers.Query.ValidateUserQuery;

public sealed record ValidateUserQuery(string UserName, string Password, string Email, int TenantId) : IQuery<string>
{
}