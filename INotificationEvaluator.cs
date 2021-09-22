using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApp
{
    public enum NotificationLevel
    {
        Information,
        Warning,
        Error,
        Critical
    }

    public interface INotificationResult
    {
        NotificationLevel Level { get; }
        string Message { get; }
    }


    public interface INotificationEvaluator
    {
        NotificationType[] SupportNotificationTypes { get; }
        Task<INotificationResult> Evaluate(INotificationRule rule, IStreamEvent streamEvent);
    }
}
