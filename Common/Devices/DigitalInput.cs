namespace Common.Devices
{
    public class DigitalInput : Device
    {
        public byte MaxValue { get; set; }
        public byte MinValue { get; set; }
        public byte InitialValue { get; set; }
        public byte Value { get; set; }

        public DigitalInput() : base() { }

        public DigitalInput(string typeOfRegister, int address, string type, string description, byte maxValue, byte minValue, byte initialValue, byte value) : base(typeOfRegister, address, type, description)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            InitialValue = initialValue;
            Value = value;
        }
    }
}
