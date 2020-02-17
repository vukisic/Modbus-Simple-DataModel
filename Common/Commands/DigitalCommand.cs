using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Commands
{
    public class DigitalCommand : DeviceCommand
    {
        public byte Value { get; set; }
    }
}
