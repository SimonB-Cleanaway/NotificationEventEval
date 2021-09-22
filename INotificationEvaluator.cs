using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApp
{
    public interface INotificationEvaluator
    {
        NotificationType[] SupportNotificationTypes { get; }
        
        Task<bool> Evaluate(INotificationRule rule, IStreamEvent streamEvent);

    }
}
