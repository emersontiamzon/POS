using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Handlers.Command.AddRoleCommand;

public record AddRoleCommand(string Role) : ICommand<bool>
{
}