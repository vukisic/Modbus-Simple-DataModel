using Common.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Devices
{
    public class DeviceValidator : IDeviceValidator
    {
        public bool Validate(Device device, double value)
        {
            if (device is AnalogInput)
            {
                var item = device as AnalogInput;
                if (value <= item.MaxValue && value >= item.MinValue)
                    return true;
                else
                    return false;
            }
            else if (device is AnalogOutput)
            {
                var item = device as AnalogOutput;
                if (value <= item.MaxValue && value >= item.MinValue)
                    return true;
                else
                    return false;
            }
            else if (device is DigitalInput)
            {
                var item = device as DigitalInput;
                if (value != item.MaxValue)
                    return true;
                else
                    return false;
            }
            else if (device is DigitalOutput)
            {

                var item = device as DigitalOutput;
                if (value != item.MaxValue)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public bool Validate(Device device)
        {
            if (device is AnalogInput)
            {
                var item = device as AnalogInput;
                if (item.Value <= item.MaxValue && item.Value >= item.MinValue)
                    return true;
                else
                    return false;
            }
            else if (device is AnalogOutput)
            {
                var item = device as AnalogOutput;
                if (item.Value <= item.MaxValue && item.Value >= item.MinValue)
                    return true;
                else
                    return false;
            }
            else if (device is DigitalInput)
            {
                var item = device as DigitalInput;
                if (item.Value != item.MaxValue)
                    return true;
                else
                    return false;
            }
            else if (device is DigitalOutput)
            {

                var item = device as DigitalOutput;
                if (item.Value != item.MaxValue)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
