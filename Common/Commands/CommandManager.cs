using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Commands
{
    public class CommandManager
    {
        private static CommandManager manager;
        private Queue<DeviceCommand> Commands { get; set; }
        private object obj;

        private CommandManager()
        {
            Commands = new Queue<DeviceCommand>();
            obj = new object();
        }

        public static CommandManager GetInstance()
        {
            if(manager == null)
            {
                if(manager == null)
                {
                    manager = new CommandManager();
                }
            }
            return manager;
        }

        public void PutCommand(DeviceCommand command)
        {
            lock(obj)
            {
                Commands.Enqueue(command);
            }
        }

        public DeviceCommand GetCommand ()
        {
            lock(obj)
            {
                return Commands.Dequeue();
            }
            
        }

        public int CommandsCount()
        {
            lock(obj)
            {
                return Commands.Count();
            }
           
        }
    }
}
