using Common.Commands;
using Common.Devices;
using Common.Logger;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CommandProcessor
    {
        private INotificationService notificationService;
        private IDeviceValidator validator;
        private CommandExecutor commandExecutor;
        private ILogger logger;

        public CommandProcessor(INotificationService notificationService, IDeviceValidator validator, CommandExecutor executor, ILogger logger)
        {
            this.notificationService = notificationService;
            this.validator = validator;
            this.logger = logger;
            this.commandExecutor = executor;
        }

        public bool ProcessCommand(Device device, double value)
        {
            if (device is AnalogInput || device is DigitalInput)
                return false;
            if (device is AnalogOutput)
            {

                if (validator.Validate(device, value))
                {
                    var analogCommand = new AnalogCommand() { Address = device.Address, Value = value };
                    commandExecutor.PutCommand(analogCommand);
                }
                else
                {
                    notificationService.ShowNotification("Error", "Value out of range!", Notifications.Wpf.NotificationType.Error);
                    logger.Warning($"Value {value} out of range for device at address {device.TypeOfRegister}{device.Address}");
                }
                return true;
            }
            else
            {
                var digitalCommand = new DigitalCommand() { Address = device.Address, Value = (byte)value };
                commandExecutor.PutCommand(digitalCommand);
                return true;
            }
        }
    }
}
