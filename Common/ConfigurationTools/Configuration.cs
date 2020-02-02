using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ConfigurationTools
{
    public class Configuration
    {
        public int Port { get; set; }
        public List<ConfigurationItem> Items { get; set; }
    }
}
