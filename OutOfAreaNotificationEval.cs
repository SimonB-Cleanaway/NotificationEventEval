using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    public class OutOfAreaNotificationEval : INotificationEvaluator
    {
        public NotificationType[] SupportNotificationTypes => new[] { NotificationType.OutOfArea };

        public Task<bool> Evaluate(INotificationRule rule, IStreamEvent streamEvent)
        {
            return Task.FromResult(false);
        }
    }
}
