using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleApp
{
    public class VehicleRegistrationRuleDetail
    {
        public ushort CheckIntervalDays { get; set; }
        public NotificationLevel Level { get; set; }
        public string Message { get; set; }
    }



    public class VehicleRegistrationNotificationEval : INotificationEvaluator
    {
        public NotificationType[] SupportNotificationTypes => new[] { NotificationType.VehicleRegistraion };

        Task<INotificationResult> INotificationEvaluator.Evaluate(INotificationRule rule, IStreamEvent streamEvent)
        {
            var details = LoaderFactory.LoadFromJson<VehicleRegistrationRuleDetail>(rule.Details);

            INotificationResult result = null;

            // Rules Evaluation
            if ((streamEvent.RegisteredTo - DateTime.Now).TotalDays < details.CheckIntervalDays)
            {
                result = new NotificationResult
                {
                    Level = details.Level,
                    Message = details.Message
                };
            }

            return Task.FromResult(result);
        }
    }
}
