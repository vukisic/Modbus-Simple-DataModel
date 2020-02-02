using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Devices
{
    public class AnalogOutput : Device
    {

        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double InitialValue { get; set; }
        public double Value { get; set; }

        public AnalogOutput() : base() { }
        public AnalogOutput(string typeOfRegister, int address, string type, string description, double maxValue, double minValue, double initialValue, double value) : base(typeOfRegister, address, type, description)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            InitialValue = initialValue;
            Value = value;
        }
    }
}
