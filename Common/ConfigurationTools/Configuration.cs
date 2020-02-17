using System.Collections.Generic;

namespace Common.ConfigurationTools
{
    public class Configuration
    {
        public int Port { get; set; }
        public int AquisitionInterval { get; set; }
        public List<ConfigurationItem> Items { get; set; }
    }
}
