using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class DigitalInputModel : DeviceModel
    {
        private byte value;

        public byte MaxValue { get; set; }
        public byte MinValue { get; set; }
        public byte InitialValue { get; set; }
        public byte Value { get { return value; } set { this.value = value; OnPropertyChanged("Value"); } }

        public DigitalInputModel() : base() { }

        public DigitalInputModel(string typeOfRegister, byte address, string type, string description, byte maxValue, byte minValue, byte initialValue, byte value) : base(typeOfRegister, address, type, description)
        {
            MaxValue = maxValue;
            MinValue = minValue;
            InitialValue = initialValue;
            Value = value;
        }
    }
}
