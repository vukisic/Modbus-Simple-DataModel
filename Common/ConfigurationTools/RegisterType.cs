using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ConfigurationTools
{
    /// <summary>
    /// DO_REG - Digital Output
    /// DI_REG - Digital Input
    /// IN_REG - Analog Input
    /// HR_INT - Analog Output
    /// </summary>
    public static class RegisterType
    {
        public const string DigitalInput = "DO_REG";
        public const string DigitalOutput = "DI_REG";
        public const string AnalogInput = "IN_REG";
        public const string AnalogOutput = "HR_INT";
    }
}
