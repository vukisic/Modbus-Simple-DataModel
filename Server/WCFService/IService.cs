using Common.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.WCFService
{
    public interface IService
    {
        AllDevices GetAllDevices();
    }
}
