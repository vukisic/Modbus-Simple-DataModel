using System.Collections.Generic;

namespace Common.Devices
{
    public class AllDevices
    {
        public List<DigitalInput> DigitalInputs { get; set; }
        public List<DigitalOutput> DigitalOutputs { get; set; }
        public List<AnalogInput> AnalogInputs { get; set; }
        public List<AnalogOutput> AnalogOutputs { get; set; }

        public AllDevices()
        {
            DigitalInputs = new List<DigitalInput>();
            DigitalOutputs = new List<DigitalOutput>();
            AnalogInputs = new List<AnalogInput>();
            AnalogOutputs = new List<AnalogOutput>();
        }
    }
}
