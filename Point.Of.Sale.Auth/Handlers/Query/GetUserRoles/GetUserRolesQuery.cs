using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Handlers.Query.GetUserRoles;

public record GetUserRolesQuery(string UserName) : IQuery<List<string>>
{
}