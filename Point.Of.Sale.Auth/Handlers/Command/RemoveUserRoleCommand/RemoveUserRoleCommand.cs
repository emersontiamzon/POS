using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Handlers.Command.RemoveUserRoleCommand;

public record RemoveUserRoleCommand(string UserName, string[] RoleName) : ICommand<bool>
{
}