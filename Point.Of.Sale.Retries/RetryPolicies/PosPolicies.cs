using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Point.Of.Sale.Retries.RetryPolicies;

public static class PosPolicies
{
    public static RetryPolicy? RetryXTimes(int retries = 1)
    {
        return Policy.Handle<Exception>().Retry(retries);
    }

    public static RetryPolicy? RetryForever()
    {
        return Policy.Handle<Exception>().RetryForever();
    }

    public static RetryPolicy? RetryActionDelegate(Action? action, int retries = 3)
    {
        if (action is null)
        {
            return null;
        }

        return Policy.Handle<Exception>().Retry(3, (exception, retryCount, context) => { action(); });
    }

    private static RetryPolicy? WaitAndRetry(int retries = 3)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetry(
                retries,
                retryAttempt => TimeSpan.FromMilliseconds(1000) * retryAttempt,
                (exception, timeSpan, context) => { Console.WriteLine(exception.Message); }
            );
    }

    public static async Task<object?> ExecuteWaitAndRetry(Func<object> action, int retries = 5)
    {
        if (WaitAndRetry(retries) is { } policy)
        {
            var result = await policy.Execute(() => Task.FromResult(action()));
        }

        return default;
    }

    public static async Task<TResult> ExecuteWaitAndRetry<TResult>(Func<Task<TResult>> action, int retries = 5)
    {
        if (WaitAndRetry(retries) is not { } policy)
        {
            return Task.FromResult<TResult>(default!).Result;
        }

        var result = await policy.Execute(() => Task.FromResult(action()));
        return await result;
    }

    public static async Task<PolicyResult<TResult>> ExecuteThenCaptureResult<TResult>(Func<Task<TResult>> action, ILogger logger, int retries = 5)
    {
        var execute = await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retries,
                retryAttempt => TimeSpan.FromMilliseconds(1000) * retryAttempt,
                (exception, timeSpan, context) => { logger.LogError(exception, exception.Message); }
            ).ExecuteAndCaptureAsync(async () => await action());

        if (execute is {Result: null, Outcome: OutcomeType.Failure})
        {
            logger.LogError(execute.FinalException, execute.FinalException.Message);
        }

        return execute;
    }

    public static async Task<PolicyResult<TResult>> ExecuteThenCaptureResult<TResult>(Func<Task<TResult>> action, int retries = 5)
    {
        var execute = await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retries,
                retryAttempt => TimeSpan.FromMilliseconds(1000) * retryAttempt,
                (exception, timeSpan, context) => { Console.WriteLine(exception.Message); }
            ).ExecuteAndCaptureAsync(async () => await action());

        return execute;
    }
}
