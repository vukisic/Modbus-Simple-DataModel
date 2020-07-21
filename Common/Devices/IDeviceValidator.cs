using Common.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Devices
{
    public interface IDeviceValidator
    {
        bool Validate(Device device, double value);
        bool Validate(Device device);
    }
}
