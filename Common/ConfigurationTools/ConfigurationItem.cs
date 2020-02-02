using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ConfigurationTools
{
    /// <summary>
    /// Represents one configuration item
    /// </summary>
    public class ConfigurationItem
    {
        public string TypeOfRegister { get; set; }
        public int NumberOfRegister { get; set; }
        public int StartAddress { get; set; }
        public int FloatingPointPosition { get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public double InitialValue { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public ConfigurationItem()
        {
           
        }
    }
}
