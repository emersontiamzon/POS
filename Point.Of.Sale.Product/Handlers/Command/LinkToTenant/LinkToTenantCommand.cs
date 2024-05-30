using Point.Of.Sale.Abstraction.Message;

namespace Point.Of.Sale.Product.Handlers.Command.LinkToTenant;

public sealed record LinkToTenantCommand(int tenantId, int entityId) : ICommand;