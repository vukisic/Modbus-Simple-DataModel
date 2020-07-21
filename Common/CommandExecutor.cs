using Common.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CommandExecutor
    {
        public void PutCommand(AnalogCommand command) => CommandManager.GetInstance().PutCommand(command);
        public void PutCommand(DigitalCommand command) => CommandManager.GetInstance().PutCommand(command);

        public void ExecuteCommands(IService proxy)
        {
            var instance = CommandManager.GetInstance();
            if (instance.CommandsCount() > 0)
            {
                for (int i = 0; i < instance.CommandsCount(); ++i)
                {
                    var command = instance.GetCommand();
                    if (command is AnalogCommand)
                        proxy.CommandAnalogs(command as AnalogCommand);
                    else
                        proxy.CommandDigitals(command as DigitalCommand);
                }
            }
        }
    }
}
