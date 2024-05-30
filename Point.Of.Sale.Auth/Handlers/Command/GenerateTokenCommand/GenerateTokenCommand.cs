using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Auth.Handlers.Command.GenerateTokenCommand;

public record GenerateTokenCommand(string UserName, string Email, int TenantId) : ICommand<string>
{
}