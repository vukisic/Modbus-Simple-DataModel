using System;

namespace Common.Devices
{
    public class AnalogInput : Device
    {
        private double value;
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double InitialValue { get; set; }
        public double Value { get { return value; } set { this.value = value;OnPropertyChanged("Value"); } }

        public AnalogInput() : base() { }

        public AnalogInput(string typeOfRegister, int address, string type, string description, double maxValue, double minValue, double initialValue, double value) : base(typeOfRegister, address, type, description)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            InitialValue = initialValue;
            Value = value;
        }
    }
}
