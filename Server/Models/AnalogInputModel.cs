namespace Server.Models
{
    public class AnalogInputModel : DeviceModel
    {
        private double value;

        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double InitialValue { get; set; }
        public double Value { get { return value; } set { this.value = value; OnPropertyChanged("Value"); } }

        public AnalogInputModel() : base() { }

        public AnalogInputModel(string typeOfRegister, int address, string type, string description, double maxValue, double minValue, double initialValue, double value) : base(typeOfRegister, address, type, description)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            InitialValue = initialValue;
            Value = value;
        }
    }
}
