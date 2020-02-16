using Server.Core;

namespace Server.Models
{
    public class DeviceModel : BindableBase
    {
        public string TypeOfRegister { get; set; }
        public int Address { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public DeviceModel() { }
        public DeviceModel(string typeOfRegister, int address, string type, string description)
        {
            TypeOfRegister = typeOfRegister;
            Address = address;
            Type = type;
            Description = description;
        }
    }
}
