using Common.Devices;
using Server.Context;
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
        private object obj;
        public DataRepository()
        {
            context = new AppDBContext();
            obj = new object();
        }

        public bool Add(AnalogInput device)
        {
            try
            {
                context.Devices.Add(device);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        public bool Add(AnalogOutput device)
        {
            try
            {
                context.Devices.Add(device);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(DigitalInput device)
        {
            try
            {
                context.Devices.Add(device);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Add(DigitalOutput device)
        {
            try
            {
                context.Devices.Add(device);
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
            lock (obj)
            {
                AllDevices devices = new AllDevices();
                var result = context.Devices.ToList();
                foreach (var item in result)
                {
                    if (item is AnalogInput)
                    {
                        var ai = item as AnalogInput;
                        devices.AnalogInputs.Add(new AnalogInput(ai.TypeOfRegister, ai.Address, ai.Type, ai.Description, ai.MaxValue, ai.MinValue, ai.InitialValue, ai.Value));
                    }
                    else if (item is AnalogOutput)
                    {
                        var ao = item as AnalogOutput;
                        devices.AnalogOutputs.Add(new AnalogOutput(ao.TypeOfRegister, ao.Address, ao.Type, ao.Description, ao.MaxValue, ao.MinValue, ao.InitialValue, ao.Value));
                    }
                    else if (item is DigitalInput)
                    {
                        var di = item as DigitalInput;
                        devices.DigitalInputs.Add(new DigitalInput(di.TypeOfRegister, di.Address, di.Type, di.Description, di.MaxValue, di.MinValue, di.InitialValue, di.Value));
                    }
                    else if (item is DigitalOutput)
                    {
                        var dout = item as DigitalOutput;
                        devices.DigitalOutputs.Add(new DigitalOutput(dout.TypeOfRegister, dout.Address, dout.Type, dout.Description, dout.MaxValue, dout.MinValue, dout.InitialValue, dout.Value));
                    }
                }
                return devices;
            }
            
        }

        public bool Update(AnalogInput device, double value)
        {
            try
            {
                lock(obj)
                {
                    var result = context.Devices.SingleOrDefault(x => x.Address == device.Address);
                    if (result != null)
                    {
                        (result as AnalogInput).Value = value;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
               
            }
            catch
            {
                return false;
            }
        }

        public bool Update(AnalogOutput device, double value)
        {
            try
            {

                lock(obj)
                {
                    var result = context.Devices.SingleOrDefault(x => x.Address == device.Address);
                    if (result != null)
                    {
                        (result as AnalogOutput).Value = value;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                
            }
            catch
            {
                return false;
            }
        }

        public bool Update(DigitalInput device, byte value)
        {
            try
            {
                lock(obj)
                {
                    var result = context.Devices.SingleOrDefault(x => x.Address == device.Address);
                    if (result != null)
                    {
                        (result as DigitalInput).Value = value;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
               
            }
            catch
            {
                return false;
            }
        }

        public bool Update(DigitalOutput device, byte value)
        {
            try
            {
                lock(obj)
                {
                    var result = context.Devices.SingleOrDefault(x => x.Address == device.Address);
                    if (result != null)
                    {
                        (result as DigitalOutput).Value = value;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
                
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
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE Devices");
        }

        public BindingList<Device> GetAllDeviceBindings()
        {
            lock(obj)
            {
                var list = context.Devices.ToList();
                var blist = new BindingList<Device>();
                foreach (var item in list)
                    blist.Add(item);
                return blist;
            }
           
        }
    }
}
