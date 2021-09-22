using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    public enum NotificationType
    {
        VehicleRegistraion,
        OutOfArea
    }


    public interface INotificationRule
    {
        NotificationType NotificationType { get; }
        string Details { get; }
    }
}
