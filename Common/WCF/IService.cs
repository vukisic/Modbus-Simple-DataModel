using Common.Commands;
using Common.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        AllDevices GetAllDevices();
        [OperationContract]
        void CommandAnalogs(AnalogCommand command);
        [OperationContract]
        void CommandDigitals(DigitalCommand command);
    }
}
