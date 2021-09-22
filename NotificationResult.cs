using System;

namespace SampleApp
{
    public class NotificationResult : INotificationResult
    {
        public NotificationLevel Level { get; set; }
        public string Message { get; set; }
    }
}
