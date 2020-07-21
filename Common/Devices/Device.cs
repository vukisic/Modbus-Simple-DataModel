using Common.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Devices
{
    public class Device : BindableBase
    {
        private string alarm;
        public string TypeOfRegister { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Address { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Alarm { get { return alarm; } set { alarm = value; OnPropertyChanged("Alarm"); } }

        public Device() { }
        public Device(string typeOfRegister, int address, string type, string description)
        {
            TypeOfRegister = typeOfRegister;
            Address = address;
            Type = type;
            Description = description;
            Alarm = AlarmType.NO_ALARM;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
