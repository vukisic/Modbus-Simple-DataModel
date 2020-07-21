using Common.Devices;
using Common.Logger;
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
        private ILogger logger;

        public AlarmProcessor(ILogger logger)
        {
            this.logger = logger;
        }
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
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {ao.Value} -> {AlarmType.NO_ALARM}");
                    return AlarmType.NO_ALARM;
                }
                else if(ao.Value > ao.MaxValue)
                {
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {ao.Value} -> {AlarmType.HIGH_ALARM}");
                    return AlarmType.HIGH_ALARM;
                }
                else
                {
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {ao.Value} -> {AlarmType.LOW_ALARM}");
                    return AlarmType.LOW_ALARM;
                }
            }
            else
            {
                var ai = device as AnalogInput;
                if (ai.Value <= ai.MaxValue && ai.Value >= ai.MinValue)
                {
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {ai.Value} -> {AlarmType.NO_ALARM}");
                    return AlarmType.NO_ALARM;
                }
                else if (ai.Value > ai.MaxValue)
                {
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {ai.Value} -> {AlarmType.HIGH_ALARM}");
                    return AlarmType.HIGH_ALARM;
                }
                else
                {
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {ai.Value} -> {AlarmType.LOW_ALARM}");
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
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {od.Value} -> {AlarmType.ABNORMAL}");
                    return AlarmType.ABNORMAL;
                }
                else
                {
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {od.Value} -> {AlarmType.NO_ALARM}");
                    return AlarmType.NO_ALARM;
                }
            }
            else
            {
                var od = device as DigitalInput;
                if (od.Value == od.MaxValue)
                {
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {od.Value} -> {AlarmType.ABNORMAL}");
                    return AlarmType.ABNORMAL;
                }
                else
                {
                    logger.Info($"Device {device.TypeOfRegister}-{device.Address} Value: {od.Value} -> {AlarmType.NO_ALARM}");
                    return AlarmType.NO_ALARM;
                }
            }
        }
    }
}
