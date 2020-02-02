using Common.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DeviceDTOs
{
    public class DeviceDTO
    {
        public string TypeOfRegister { get; set; }
        public int Address { get; set; }
        public string TimeStamp { get; set; }

        public DeviceDTO(string typeOfRegister, int address, string timeStamp)
        {
            TypeOfRegister = typeOfRegister;
            Address = address;
            TimeStamp = timeStamp;
        }

        public override bool Equals(object obj)
        {
            if(obj is Device)
            {
                var device = obj as Device;
                return device.Address == Address && device.TypeOfRegister == device.TypeOfRegister;
            }
            else
            {

                var device = obj as DeviceDTO;
                return device.Address == Address && device.TypeOfRegister == device.TypeOfRegister && device.TimeStamp == TimeStamp;
            }

            return base.Equals(obj);
        }
    }
}
