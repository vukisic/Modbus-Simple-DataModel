using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Devices
{
    public class Device
    {
        public string TypeOfRegister { get; set; }
        public int Address { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public Device() { }
        public Device(string typeOfRegister, int address, string type, string description)
        {
            TypeOfRegister = typeOfRegister;
            Address = address;
            Type = type;
            Description = description;
        }
    }
}
