using Common.Devices;
using Common.ConfigurationTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Converters
{
	public class ConfigurationDevicesConverter
	{
		public AllDevices ConvertToDevices(Configuration config)
		{
			try
			{
				AllDevices devs = new AllDevices();
				foreach (var item in config.Items)
				{
					if(item.TypeOfRegister == RegisterType.DigitalInput)
					{
						for (int i = 0; i < item.NumberOfRegister; i++)
						{
							devs.DigitalInputs.Add(new DigitalInput()
							{
								Description = item.Description,
								InitialValue = (byte)item.InitialValue,
								MaxValue = (byte)item.MaxValue,
								MinValue = (byte)item.MinValue,
								Type = item.Type,
								TypeOfRegister = item.TypeOfRegister,
								Value = (byte)item.InitialValue,
								Address = item.StartAddress + i
							});
						}
					}
					else if (item.TypeOfRegister == RegisterType.DigitalOutput)
					{
						for (int i = 0; i < item.NumberOfRegister; i++)
						{
							devs.DigitalOutputs.Add(new DigitalOutput()
							{
								Description = item.Description,
								InitialValue = (byte)item.InitialValue,
								MaxValue = (byte)item.MaxValue,
								MinValue = (byte)item.MinValue,
								Type = item.Type,
								TypeOfRegister = item.TypeOfRegister,
								Value = (byte)item.InitialValue,
								Address = item.StartAddress + i
							});
						}
					}
					else if (item.TypeOfRegister == RegisterType.AnalogInput)
					{
						for (int i = 0; i < item.NumberOfRegister; i++)
						{
							devs.AnalogInputs.Add(new AnalogInput()
							{
								Description = item.Description,
								InitialValue = item.InitialValue,
								MaxValue = item.MaxValue,
								MinValue = item.MinValue,
								Type = item.Type,
								TypeOfRegister = item.TypeOfRegister,
								Value = item.InitialValue,
								Address = item.StartAddress + i
							});
						}

					}
					else if (item.TypeOfRegister == RegisterType.AnalogOutput)
					{
						for (int i = 0; i < item.NumberOfRegister; i++)
						{
							devs.AnalogOutputs.Add(new AnalogOutput()
							{
								Description = item.Description,
								InitialValue = item.InitialValue,
								MaxValue = item.MaxValue,
								MinValue = item.MinValue,
								Type = item.Type,
								TypeOfRegister = item.TypeOfRegister,
								Value = item.InitialValue,
								Address = item.StartAddress + i
							});
						}

					}
				}
				return devs;
			}
			catch (Exception ex)
			{

				return null;
			}
		}
	}
}
