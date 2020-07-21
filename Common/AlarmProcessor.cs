using Common.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common
{
    public class AlarmProcessor
    {
        public string ProcessDevice(Device device)
        {
            if (device is AnalogInput || device is AnalogOutput)
                return ProcessAnalogDevice(device);
            else
                return ProcessDigitalDevice(device);
        }

        private string ProcessAnalogDevice(Device device)
        {
            if(device is AnalogOutput)
            {
                var ao = device as AnalogOutput;
                if(ao.Value <= ao.MaxValue && ao.Value >= ao.MinValue)
                {
                    return AlarmType.NO_ALARM;
                }
                else if(ao.Value > ao.MaxValue)
                {
                    return AlarmType.HIGH_ALARM;
                }
                else
                {
                    return AlarmType.LOW_ALARM;
                }
            }
            else
            {
                var ai = device as AnalogInput;
                if (ai.Value <= ai.MaxValue && ai.Value >= ai.MinValue)
                {
                    return AlarmType.NO_ALARM;
                }
                else if (ai.Value > ai.MaxValue)
                {
                    return AlarmType.HIGH_ALARM;
                }
                else
                {
                    return AlarmType.LOW_ALARM;
                }
            }

        }

        private string ProcessDigitalDevice(Device device)
        {
            if (device is DigitalOutput)
            {
                var od = device as DigitalOutput;
                if (od.Value == od.MaxValue)
                {
                    return AlarmType.ABNORMAL;
                }
                else
                {
                    return AlarmType.NO_ALARM;
                }
            }
            else
            {
                var od = device as DigitalInput;
                if (od.Value == od.MaxValue)
                {
                    return AlarmType.ABNORMAL;
                }
                else
                {
                    return AlarmType.NO_ALARM;
                }
            }
        }
    }
}
