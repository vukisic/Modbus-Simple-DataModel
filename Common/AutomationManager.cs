using Common.Commands;
using Common.Devices;
using Common.Logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class AutomationManager
    {
        private CommandExecutor executor;
        private ObservableCollection<Device> devices;
        private ILogger logger;

        public AutomationManager(CommandExecutor executor, ObservableCollection<Device> devices, ILogger logger)
        {
            this.executor = executor;
            this.devices = devices;
            this.logger = logger;
        }

        public void DoWork()
        {
            logger.Debug("Automation started!");
            DigitalOutput output1 = FindByAddress(1001) as DigitalOutput;
            if (output1.Value == 1)
                executor.PutCommand(new DigitalCommand() { Address = 1001, Value = 0 });

        }

        private Device FindByAddress(int address)
        {
            return devices.SingleOrDefault(x => x.Address == address);
        }
    }
}
