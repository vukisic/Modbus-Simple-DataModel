using Common.Commands;
using Common.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.WCFService
{
    public class Service : IService
    {
        public void CommandAnalogs(AnalogCommand command)
        {
            throw new NotImplementedException();
        }

        public void CommandDigitals(DigitalCommand command)
        {
            throw new NotImplementedException();
        }

        public AllDevices GetAllDevices()
        {
            throw new NotImplementedException();
        }
    }
}
