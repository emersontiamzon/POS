using MediatR;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Events.Handlers.Command.LogAuditAction;

public record LogAuditActionCommand(string User, List<Dictionary<string, object?>> LogData) : IRequest<IFluentResults>;
