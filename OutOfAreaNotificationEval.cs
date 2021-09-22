using System;
using System.Threading.Tasks;

namespace SampleApp
{
    public class OutOfAreaNotificationEval : INotificationEvaluator
    {
        public NotificationType[] SupportNotificationTypes => new[] { NotificationType.OutOfArea };

        Task<INotificationResult> INotificationEvaluator.Evaluate(INotificationRule rule, IStreamEvent streamEvent)
        {
            return Task.FromResult<INotificationResult>(null);
        }
    }
}
