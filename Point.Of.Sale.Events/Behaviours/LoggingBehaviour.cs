using MediatR;
using Microsoft.Extensions.Logging;
using Point.Of.Sale.Shared.FluentResults;

namespace Point.Of.Sale.Events.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IFluentResults
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logging;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logging)
    {
        _logging = logging;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logging.LogInformation("Starting request {@RequestName}, {@DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);

        var result = await next();

        if (result is {Status: FluentResultsStatus.Failure})
        {
            _logging.LogError("Request Failure {@RequestName}, {@Error}, {@DateTimeUtc}", typeof(TRequest).Name, result.Messages, DateTime.UtcNow);
        }

        _logging.LogInformation("Completing request {@RequestName}, {@DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);

        return result;
    }
}
