using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
    public enum VehicleStatus
    {
        Available,
        BeingServiced
    }

    public interface IStreamEvent
    {
        string VehicleRego { get; }
        DateTime RegisteredTo { get; }
        VehicleStatus VehicleStatus { get; }
    }
}
