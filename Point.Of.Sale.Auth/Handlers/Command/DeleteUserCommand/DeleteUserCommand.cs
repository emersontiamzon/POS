using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Handlers.Command.DeleteUserCommand;

public record DeleteUserCommand(string UserName) : ICommand<bool>
{
}