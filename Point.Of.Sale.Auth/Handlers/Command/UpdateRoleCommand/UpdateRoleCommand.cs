using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Handlers.Command.UpdateRoleCommand;

public record UpdateRoleCommand(string RoleName, string RoleNormalizeName) : ICommand<bool>
{
}