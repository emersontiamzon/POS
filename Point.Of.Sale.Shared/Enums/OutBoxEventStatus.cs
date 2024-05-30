using System.ComponentModel;

namespace Point.Of.Sale.Shared.Enums;

public enum OutBoxEventStatus
{
    [Description("Cancelled")] Cancelled = -1,
    [Description("ReadyForProcessing")] ReadyForProcessing,
    [Description("CurrentlyProcessing")] CurrentlyProcessing,
    [Description("CompletedSuccessful")] CompletedSuccessful,
    [Description("CompletedWithFailure")] CompletedWithFailure,
}
