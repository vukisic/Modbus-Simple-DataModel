using Server.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class DeviceModel : BindableBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Address { get; set; }
        public string TypeOfRegister { get; set; }
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
