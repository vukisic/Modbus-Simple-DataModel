using Caliburn.Micro;
using Client.Validator;
using Common.Commands;
using Common.ConfigurationTools;
using Common.Converters;
using Common.Devices;
using Common.Services;
using Server.WCFService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.ViewModels
{
    public class MainWindowViewModel : Conductor<object>, INotifyPropertyChanged
    {
        #region Properties
        private BindingList<Device> items;
        public BindingList<Device> Items { get { return items; } set { items = value; OnPropertyChanged("Items"); } }

        private int servicePort;
        private int aquisitionInterval;
        private ChannelFactory<IService> factory;
        private IService proxy;
        private Thread aquisitor;
        private bool endSignal = true;

        private INotificationService notificationService;
        private IDeviceValidator validator;
        private DataGrid grid;
        private bool connected;
        public bool Connected { get { return connected; } set { connected = value; OnPropertyChanged("Connected"); } }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        public MainWindowViewModel(IDeviceValidator validator, INotificationService notificationService)
        {
            this.validator = validator;
            this.notificationService = notificationService;
            Items = new BindingList<Device>();
            Connected = false;

            try
            {
                LoadFromConfiguration();
                factory = new ChannelFactory<IService>(new NetTcpBinding(), $"net.tcp://localhost:{servicePort}/WCFService");
                proxy = factory.CreateChannel();
                Connected = false;
            }
            catch (Exception ex)
            {

                notificationService.ShowNotification("Server", $"Error: {ex.Message}, ST: {ex.StackTrace}", Notifications.Wpf.NotificationType.Error);
            }


        }

        #region Helpers
        public void LoadFromConfiguration()
        {
            ConfigurationParser parser = new ConfigurationParser("../../../confguration.json");
            var config = parser.Read();
            servicePort = config.Port;
            aquisitionInterval = config.AquisitionInterval * 1000;
            var converter = new ConfigurationDevicesConverter();
            var devs = converter.ConvertToDevices(config);

            foreach (var item in devs.DigitalInputs)
            {
                Items.Add(item);
            }
            foreach (var item in devs.DigitalOutputs)
            {
                Items.Add(item);
            }
            foreach (var item in devs.AnalogInputs)
            {
                Items.Add(item);
            }
            foreach (var item in devs.AnalogOutputs)
            {
                Items.Add(item);
            }
        }

        public void CheckRowStatus()
        {
            foreach (var item in Items)
            {
                var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(item);
                if (validator.Validate(item as Device))
                    row.Background = Brushes.MediumSeaGreen;
                else
                    row.Background = Brushes.IndianRed;
            }
        }
        #endregion

        #region Commands & Events

        public void SaveChanges(DataGridCellEditEndingEventArgs args)
        {
            var device = args.EditingElement.DataContext as Device;
            var value = Convert.ToDouble((args.EditingElement as TextBox).Text);
            var index = args.Row.GetIndex();
            var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(device);

            if(device is AnalogInput || device is DigitalInput)
            {
                if(device is AnalogInput)
                {
                    args.Cancel = true;
                    (grid.Columns[5].GetCellContent(row) as TextBox).Background = row.Background;
                    (grid.Columns[5].GetCellContent(row) as TextBox).Text = (Items[index] as AnalogInput).Value.ToString();
                }
                else
                {
                    args.Cancel = true;
                    (grid.Columns[5].GetCellContent(row) as TextBox).Background = row.Background;
                    (grid.Columns[5].GetCellContent(row) as TextBox).Text = (Items[index] as DigitalInput).Value.ToString();
                }
                notificationService.ShowNotification("Error", "Cannot command on input devices!", Notifications.Wpf.NotificationType.Error);
                return;
            }
            else
            {
                if(device is AnalogOutput)
                {
                    var analogCommand = new AnalogCommand() { Address = device.Address, Value = value };
                    CommandManager.GetInstance().PutCommand(analogCommand);
                }
                else
                {
                    var digitalCommand = new DigitalCommand() { Address = device.Address, Value = (byte)value };
                    CommandManager.GetInstance().PutCommand(digitalCommand);
                }
            }
        }

        public void Load(RoutedEventArgs args)
        {
            grid = ((args.OriginalSource as Grid).Children[0] as DataGrid);
            CheckRowStatus();
            aquisitor = new Thread(Aquisitor);
            aquisitor.Start();
            notificationService.ShowNotification("Client", $"Started", Notifications.Wpf.NotificationType.Success);
        }

        public void Close(object args)
        {
            endSignal = false;
            aquisitor.Abort();
            aquisitor = null;
        }
        #endregion

        #region Thread
        public void Aquisitor()
        {
            while(endSignal)
            {
                try
                {
                    var devices = proxy.GetAllDevices();
                    grid.Dispatcher.Invoke(() =>
                    {
                        Update(devices);
                    });
                    var instance = CommandManager.GetInstance();
                    if (instance.CommandsCount() > 0)
                    {
                        for (int i = 0; i < instance.CommandsCount(); ++i)
                        {
                            var command = instance.GetCommand();
                            if (command is AnalogCommand)
                                proxy.CommandAnalogs(command as AnalogCommand);
                            else
                                proxy.CommandDigitals(command as DigitalCommand);
                        }
                    }

                    devices = proxy.GetAllDevices();
                    grid.Dispatcher.Invoke(() =>
                    {
                        Update(devices);
                    });
                    Connected = true;
                }
                catch (Exception)
                {

                    Connected = false;
                    proxy = factory.CreateChannel();
                }
                

                Thread.Sleep(aquisitionInterval);
            }
        }

        public void Update(AllDevices devices)
        {
            foreach (var item in devices.DigitalInputs)
            {
                if((Items.Single(x => x.Address == item.Address) as DigitalInput).Value != item.Value)
                    (Items.Single(x => x.Address == item.Address) as DigitalInput).Value = item.Value;
            }
            foreach (var item in devices.DigitalOutputs)
            {
                if((Items.Single(x => x.Address == item.Address) as DigitalOutput).Value != item.Value)
                    (Items.Single(x => x.Address == item.Address) as DigitalOutput).Value = item.Value;
            }
            foreach (var item in devices.AnalogInputs)
            {
                if((Items.Single(x => x.Address == item.Address) as AnalogInput).Value != item.Value)
                    (Items.Single(x => x.Address == item.Address) as AnalogInput).Value = item.Value;
            }
            foreach (var item in devices.AnalogOutputs)
            {
                if((Items.Single(x => x.Address == item.Address) as AnalogOutput).Value != item.Value)
                    (Items.Single(x => x.Address == item.Address) as AnalogOutput).Value = item.Value;
            }
            CheckRowStatus();
        }
        #endregion

        #region PropertyChanged
        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
