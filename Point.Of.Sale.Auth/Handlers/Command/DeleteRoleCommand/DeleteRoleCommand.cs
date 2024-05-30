using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Handlers.Command.DeleteRoleCommand;

public record DeleteRoleCommand(string RoleName) : ICommand<bool>
{
}