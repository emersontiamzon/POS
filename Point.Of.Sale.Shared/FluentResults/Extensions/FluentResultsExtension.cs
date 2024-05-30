namespace Point.Of.Sale.Shared.FluentResults.Extension;

public static class FluentResultsExtension
{
    public static bool IsSuccess(this IFluentResults result)
    {
        return result is { } fluentResults && fluentResults.Status == FluentResultsStatus.Success;
    }

    public static bool IsNotFound(this IFluentResults result)
    {
        return result is { } fluentResults && fluentResults.Status == FluentResultsStatus.NotFound;
    }

    public static bool IsFailure(this IFluentResults result)
    {
        return result is { } fluentResults && fluentResults.Status == FluentResultsStatus.Failure;
    }

    public static bool IsBadRequest(this IFluentResults result)
    {
        return result is { } fluentResults && fluentResults.Status == FluentResultsStatus.BadRequest;
    }

    public static bool IsNotFoundOrBadRequest(this IFluentResults result)
    {
        return result is { } fluentResults && (fluentResults.Status == FluentResultsStatus.BadRequest || fluentResults.Status == FluentResultsStatus.NotFound || fluentResults.Status == FluentResultsStatus.Failure);
    }

    public static IFluentResults OnNotFound(this IFluentResults result, Func<IFluentResults> func)
    {
        if (result is { } fluentResults && fluentResults.Status == FluentResultsStatus.NotFound)
        {
            return func();
        }

        return result;
    }

    // public static IFluentResults OnFailure(this IFluentResults result, Func<IFluentResults> func)
    // {
    //     if (result is { } fluentResults && fluentResults.Status == FluentResultsStatus.Failure)
    //     {
    //         return func();
    //     }
    //
    //     return result;
    // }
    //
    // public static IFluentResults OnFailure(this IFluentResults result, Action func)
    // {
    //     if (result is { } fluentResults && fluentResults.Status == FluentResultsStatus.Failure)
    //     {
    //         func();
    //     }
    //
    //     return result;
    // }
    //
    // public static IFluentResults OnSuccess(this IFluentResults result, Func<IFluentResults> func)
    // {
    //     if (result is { } fluentResults && fluentResults.Status == FluentResultsStatus.Success)
    //     {
    //         return func();
    //     }
    //
    //     return result;
    // }
}
