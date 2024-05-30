using Point.Of.Sale.Abstraction.Message;
using Point.Of.Sale.Auth.Models;

namespace Point.Of.Sale.Auth.Handlers.Query.GetUsers;

public record GetUsersQuery : IQuery<List<ServiceUserResponse>>
{
}