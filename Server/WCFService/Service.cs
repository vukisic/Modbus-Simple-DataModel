using Caliburn.Micro;
using Common.Commands;
using Common.Devices;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.WCFService
{
    public class Service : IService
    {
        private IDataRepository repo;

        public Service()
        {
            repo = IoC.Get<IDataRepository>();
        }

        public void CommandAnalogs(AnalogCommand command)
        {
            var all = repo.GetAllDeviceBindings();
            var result = all.SingleOrDefault(x => x.Address == command.Address);
            if(result != null && result is AnalogOutputModel)
            {
                repo.Update(result as AnalogOutputModel, command.Value);
            }
           
        }

        public void CommandDigitals(DigitalCommand command)
        {
            var all = repo.GetAllDeviceBindings();
            var result = all.SingleOrDefault(x => x.Address == command.Address);
            if (result != null && result is DigitalOutputModel)
            {
                repo.Update(result as DigitalOutputModel, command.Value);
            }
        }

        public AllDevices GetAllDevices()
        {
            return repo.GetAll();
        }
    }
}
