using Common.Commands;
using Common.ConfigurationTools;
using Common.Converters;
using Server.WCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Test_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            IService proxy;

            ChannelFactory<IService> factory = new ChannelFactory<IService>(new NetTcpBinding(), "net.tcp://localhost:5000/WCFService");
            proxy = factory.CreateChannel();

            int op = 1;

            do
            {
                Console.WriteLine("1. All");
                Console.WriteLine("2. Command Digital");
                Console.WriteLine("3. Command Analog");
                Console.WriteLine("0. Exit");

                op = Int32.Parse(Console.ReadLine());

                switch (op)
                {
                    case 1:
                        var result = proxy.GetAllDevices();
                        foreach (var item in result.DigitalInputs)
                        {
                            Console.WriteLine($"{item.Description} {item.Value}");
                        }
                        foreach (var item in result.DigitalOutputs)
                        {
                            Console.WriteLine($"{item.Description} {item.Value}");
                        }
                        foreach (var item in result.AnalogInputs)
                        {
                            Console.WriteLine($"{item.Description} {item.Value}");
                        }
                        foreach (var item in result.AnalogOutputs)
                        {
                            Console.WriteLine($"{item.Description} {item.Value}");
                        }
                        break;
                    case 2:
                        Console.WriteLine();
                        Console.Write("Address");
                        var address = Int32.Parse(Console.ReadLine());
                        Console.Write("Value");
                        var value = Int32.Parse(Console.ReadLine());
                        DigitalCommand command = new DigitalCommand() { Address = address, Value = (byte)value };
                        proxy.CommandDigitals(command);
                        break;
                    case 3:
                        Console.WriteLine();
                        Console.Write("Address");
                        var addr = Int32.Parse(Console.ReadLine());
                        Console.Write("Value");
                        var val = Int32.Parse(Console.ReadLine());
                        AnalogCommand cmd = new AnalogCommand() { Address = addr, Value = val };
                        proxy.CommandAnalogs(cmd);
                        break;
                    case 0:break;
                    default: Console.WriteLine("Unknown Command!");break;
                }


            } while (op != 0);

            proxy = null;
            Console.ReadLine();

        }
    }
}
