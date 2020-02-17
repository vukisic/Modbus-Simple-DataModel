using Common.Devices;
using Server.Context;
using Server.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services
{
    public class DataRepository : IDataRepository
    {
        private AppDBContext context;
        public DataRepository()
        {
            context = new AppDBContext();
        }

        public bool Add(AnalogInputModel device)
        {
            try
            {
                context.DeviceModels.Add(device);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        public bool Add(AnalogOutputModel device)
        {
            try
            {
                context.DeviceModels.Add(device);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(DigitalInputModel device)
        {
            try
            {
                context.DeviceModels.Add(device);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(DigitalOutputModel device)
        {
            try
            {
                context.DeviceModels.Add(device);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public AllDevices GetAll()
        {
            AllDevices devices = new AllDevices();
            var result = context.DeviceModels.ToList();
            foreach (var item in result)
            {
                if(item is AnalogInputModel)
                {
                    var ai = item as AnalogInputModel;
                    devices.AnalogInputs.Add(new AnalogInput(ai.TypeOfRegister,ai.Address,ai.Type,ai.Description,ai.MaxValue,ai.MinValue,ai.InitialValue,ai.Value));
                }
                else if (item is AnalogOutputModel)
                {
                    var ao = item as AnalogOutputModel;
                    devices.AnalogOutputs.Add(new AnalogOutput(ao.TypeOfRegister, ao.Address, ao.Type, ao.Description, ao.MaxValue, ao.MinValue, ao.InitialValue, ao.Value));
                }
                else if (item is DigitalInputModel)
                {
                    var di = item as DigitalInputModel;
                    devices.DigitalInputs.Add(new DigitalInput(di.TypeOfRegister, di.Address, di.Type, di.Description, di.MaxValue, di.MinValue, di.InitialValue, di.Value));
                }
                else if (item is DigitalOutputModel)
                {
                    var dout = item as DigitalOutputModel;
                    devices.DigitalOutputs.Add(new DigitalOutput(dout.TypeOfRegister, dout.Address, dout.Type, dout.Description, dout.MaxValue, dout.MinValue, dout.InitialValue, dout.Value));
                }
            }
            return devices;
        }

        public bool Update(AnalogInputModel device, double value)
        {
            try
            {
                var result = context.DeviceModels.SingleOrDefault(x => x.Address == device.Address);
                if(result != null)
                {
                    (result as AnalogInputModel).Value = value;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(AnalogOutputModel device, double value)
        {
            try
            {
                var result = context.DeviceModels.SingleOrDefault(x => x.Address == device.Address);
                if (result != null)
                {
                    (result as AnalogOutputModel).Value = value;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(DigitalInputModel device, byte value)
        {
            try
            {
                var result = context.DeviceModels.SingleOrDefault(x => x.Address == device.Address);
                if (result != null)
                {
                    (result as DigitalInputModel).Value = value;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(DigitalOutputModel device, byte value)
        {
            try
            {
                var result = context.DeviceModels.SingleOrDefault(x => x.Address == device.Address);
                if (result != null)
                {
                    (result as DigitalOutputModel).Value = value;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void RemoveAll()
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE AnalogInputs");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE AnalogOutputs");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE DigitalInputs");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE DigitalOutputs");
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE DeviceModels");
        }

        public BindingList<DeviceModel> GetAllDeviceBindings()
        {
            var list = context.DeviceModels.ToList();
            var blist = new BindingList<DeviceModel>();
            foreach (var item in list)
                blist.Add(item);
            return blist;
        }
    }
}
