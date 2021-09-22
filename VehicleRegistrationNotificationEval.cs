using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApp
{
    public class VehicleRegistrationNotificationEval : INotificationEvaluator
    {
        public NotificationType[] SupportNotificationTypes => new[] { NotificationType.VehicleRegistraion };
        public Task<bool> Evaluate(INotificationRule rule, IStreamEvent streamEvent)
        {
            return Task.FromResult(true);
        }
    }
}
