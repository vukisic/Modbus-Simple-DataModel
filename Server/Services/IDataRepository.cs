using Common.Devices;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IDataRepository
    {
        bool Add(AnalogInputModel device);
        bool Add(AnalogOutputModel device);
        bool Add(DigitalInputModel device);
        bool Add(DigitalOutputModel device);

        bool Update(AnalogInputModel device, double value);
        bool Update(AnalogOutputModel device, double value);
        bool Update(DigitalInputModel device, byte value);
        bool Update(DigitalOutputModel device, byte value);

        AllDevices GetAll();

        void RemoveAll();
    }
}
