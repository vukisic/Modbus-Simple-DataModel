using Newtonsoft.Json;
using System;
using System.IO;

namespace Common.ConfigurationTools
{
    public class ConfigurationParser
    {
        private string path;

        public ConfigurationParser(string path)
        {
            this.path = path;
        }

        public Configuration Read()
        {
            try
            {
                string JSONstring = File.ReadAllText(path);
                Configuration config = JsonConvert.DeserializeObject<Configuration>(JSONstring);
                return config;
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }

        public bool Write(Configuration config)
        {
            try
            {
                string JSONout = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(path, JSONout);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            

        }
    }
}
