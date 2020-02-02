using Common.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DeviceDTOs
{
    public class AnalogDeviceDTO : DeviceDTO
    {
        public double Value { get; set; }

        public AnalogDeviceDTO(string typeOfRegister, int address, double value, string timeStamp) : base(typeOfRegister, address, timeStamp)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
