using Common.Devices;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EventUpdateArgs
    {
        public AllDevices Devices { get; set; }
        public bool Connected { get; set; }
    }
}
