using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Events.Repository;
using Point.Of.Sale.Persistence.Models;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Events.Handlers.Command.LogAuditAction;

public class LogAuditActionCommandHandler : IRequestHandler<LogAuditActionCommand, IFluentResults>
{
    private readonly ILogger<LogAuditActionCommandHandler> _logger;
    private readonly IRepository _repository;

    public LogAuditActionCommandHandler(ILogger<LogAuditActionCommandHandler> logger, IRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IFluentResults> Handle(LogAuditActionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var logData = new AuditLog
            {
                EntityName = string.Empty,
                EntityId = string.Empty,
                Action = "Api Request",
                Changes = JsonSerializer.Serialize(request.LogData),
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = request.User,
            };

            return ResultsTo.Something(await _repository.Add(logData, cancellationToken) is {Value.Count: > 0});
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error logging audit action");
            return ResultsTo.Failure("Error logging audit action").WithMessage(e.Message).FromException(e);
        }
    }
}
