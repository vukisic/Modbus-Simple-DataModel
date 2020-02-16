using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Validators
{
    public interface IDeviceValidator
    {
        bool Validate(DeviceModel device, double value);
        bool Validate(DeviceModel device);
    }
}
