using System.Collections.Generic;

namespace Server.Models
{
    public class AllDevicesModel
    {
        public List<DigitalInputModel> DigitalInputs { get; set; }
        public List<DigitalOutputModel> DigitalOutputs { get; set; }
        public List<AnalogInputModel> AnalogInputs { get; set; }
        public List<AnalogOutputModel> AnalogOutputs { get; set; }

        public AllDevicesModel()
        {
            DigitalInputs = new List<DigitalInputModel>();
            DigitalOutputs = new List<DigitalOutputModel>();
            AnalogInputs = new List<AnalogInputModel>();
            AnalogOutputs = new List<AnalogOutputModel>();
        }
    }
}
