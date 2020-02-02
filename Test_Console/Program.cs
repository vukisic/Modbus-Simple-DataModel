using Common.ConfigurationTools;
using Common.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //ConfigurationItem item1 = new ConfigurationItem() { Description = "DigIn", FloatingPointPosition = 0, InitialValue = 1, MaxValue = 1, MinValue = 0, NumberOfRegister = 10, StartAddress = 0, Type = "DI", TypeOfRegister = RegisterType.DigitalInput };
            //Configuration config = new Configuration()
            //{
            //    Port = 5000,
            //    Items = new List<ConfigurationItem>() { item1 }
            //};

            ConfigurationParser parser = new ConfigurationParser("../../../confguration.json");
            //parser.Write(config);

            var config = parser.Read();
            Console.WriteLine($"Port: {config.Port}");
            foreach (var item in config.Items)
            {
                Console.WriteLine($"{item.TypeOfRegister} {item.NumberOfRegister} {item.StartAddress} {item.FloatingPointPosition} {item.MinValue} {item.MaxValue} {item.InitialValue} {item.Type} {item.Description}");
            }

            Console.WriteLine();

            ConfigurationDevicesConverter converter = new ConfigurationDevicesConverter();
            var devs = converter.ConvertToDevices(config);
           
            foreach (var item in devs.DigitalInputs)
            {
                Console.WriteLine($"{item.Type} {item.Address} {item.Value}");
            }
            foreach (var item in devs.DigitalOutputs)
            {
                Console.WriteLine($"{item.Type} {item.Address} {item.Value}");
            }
            foreach (var item in devs.AnalogInputs)
            {
                Console.WriteLine($"{item.Type} {item.Address} {item.Value}");
            }
            foreach (var item in devs.AnalogOutputs)
            {
                Console.WriteLine($"{item.Type} {item.Address} {item.Value}");
            }

            Console.ReadLine();

        }
    }
}
