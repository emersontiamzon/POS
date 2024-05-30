using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Person.Handlers.Command.LinkToTenant;

public sealed record LinkToTenantCommand(int tenantId, int entityId) : ICommand;