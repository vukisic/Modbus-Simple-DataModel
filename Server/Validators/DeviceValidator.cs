using Server.Models;

namespace Server.Validators
{
    public class DeviceValidator : IDeviceValidator
    {
        public bool Validate(DeviceModel device, double value)
        {
            if (device is AnalogInputModel)
            {
                var item = device as AnalogInputModel;
                if (value <= item.MaxValue && value >= item.MinValue)
                    return true;
                else
                    return false;
            }
            else if (device is AnalogOutputModel)
            {
                var item = device as AnalogOutputModel;
                if (value <= item.MaxValue && value >= item.MinValue)
                    return true;
                else
                    return false;
            }
            else if (device is DigitalInputModel)
            {
                var item = device as DigitalInputModel;
                if (value != item.MaxValue)
                    return true;
                else
                    return false;
            }
            else if (device is DigitalOutputModel)
            {

                var item = device as DigitalOutputModel;
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

        public bool Validate(DeviceModel device)
        {
            if (device is AnalogInputModel)
            {
                var item = device as AnalogInputModel;
                if (item.Value <= item.MaxValue && item.Value >= item.MinValue)
                    return true;
                else
                    return false;
            }
            else if (device is AnalogOutputModel)
            {
                var item = device as AnalogOutputModel;
                if (item.Value <= item.MaxValue && item.Value >= item.MinValue)
                    return true;
                else
                    return false;
            }
            else if (device is DigitalInputModel)
            {
                var item = device as DigitalInputModel;
                if (item.Value != item.MaxValue)
                    return true;
                else
                    return false;
            }
            else if (device is DigitalOutputModel)
            {

                var item = device as DigitalOutputModel;
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
