using Common.ConfigurationTools;
using System;

namespace Server.Models
{
	public class ConfigurationToModelsConverter
	{
		public AllDevicesModel ConvertToDevices(Configuration config)
		{
			try
			{
				AllDevicesModel devs = new AllDevicesModel();
				foreach (var item in config.Items)
				{
					if (item.TypeOfRegister == RegisterType.DigitalInput)
					{
						for (int i = 0; i < item.NumberOfRegister; i++)
						{
							devs.DigitalInputs.Add(new DigitalInputModel()
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
							devs.DigitalOutputs.Add(new DigitalOutputModel()
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
							devs.AnalogInputs.Add(new AnalogInputModel()
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
							devs.AnalogOutputs.Add(new AnalogOutputModel()
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
