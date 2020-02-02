using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Devices
{
    public class DigitalOutput : Device
    {
        public byte MaxValue { get; set; }
        public byte MinValue { get; set; }
        public byte InitialValue { get; set; }
        public byte Value { get; set; }

        public DigitalOutput() : base() { }

        public DigitalOutput(string typeOfRegister, byte address, string type, string description, byte maxValue, byte minValue, byte initialValue, byte value) : base(typeOfRegister, address, type, description)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            InitialValue = initialValue;
            Value = value;
        }
    }
}
