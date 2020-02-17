using Common.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IDataRepository
    {
        bool Add(AnalogInput device);
        bool Add(AnalogOutput device);
        bool Add(DigitalInput device);
        bool Add(DigitalOutput device);

        bool Update(AnalogInput device, double value);
        bool Update(AnalogOutput device, double value);
        bool Update(DigitalInput device, byte value);
        bool Update(DigitalOutput device, byte value);

        AllDevices GetAll();

        BindingList<Device> GetAllDeviceBindings();

        void RemoveAll();
    }
}
