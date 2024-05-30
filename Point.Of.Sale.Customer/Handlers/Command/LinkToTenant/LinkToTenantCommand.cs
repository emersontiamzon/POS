using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Customer.Handlers.Command.LinkToTenant;

public sealed record LinkToTenantCommand(int tenantId, int entityId) : ICommand;