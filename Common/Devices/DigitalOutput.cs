namespace Common.Devices
{
    public class DigitalOutput : Device
    {
        private byte value;
        public byte MaxValue { get; set; }
        public byte MinValue { get; set; }
        public byte InitialValue { get; set; }
        public byte Value { get { return value; } set { this.value = value; OnPropertyChanged("Value"); } }

        public DigitalOutput() : base() { }

        public DigitalOutput(string typeOfRegister, int address, string type, string description, byte maxValue, byte minValue, byte initialValue, byte value) : base(typeOfRegister, address, type, description)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            InitialValue = initialValue;
            Value = value;
        }
    }
}
