using Caliburn.Micro;
using Common;
using Common.Commands;
using Common.ConfigurationTools;
using Common.Converters;
using Common.Devices;
using Common.Services;
using Server.WCFService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Device> items;
        public ObservableCollection<Device> Items { get { return items; } set { items = value; OnPropertyChanged("Items"); } }

        private int servicePort;
        private int acquisitionInterval;
        private ChannelFactory<IService> factory;
        private IService proxy;

        private INotificationService notificationService;
        private CommandExecutor commandExecutor;
        private CommandProcessor commandProcessor;
        private IDeviceValidator validator;
        private Acquisitor acquisitor;
        private DataGrid grid;
        private AlarmProcessor alarmProcessor;
        private bool connected;
        private event EventHandler<EventUpdateArgs> UpdateEvent;
        public bool Connected { get { return connected; } set { connected = value; OnPropertyChanged("Connected"); } }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion

        public MainWindowViewModel(IDeviceValidator validator, INotificationService notificationService)
        {
            this.validator = validator;
            this.notificationService = notificationService;
            Items = new ObservableCollection<Device>();
            Connected = false;
            commandExecutor = new CommandExecutor();
            commandProcessor = new CommandProcessor(notificationService, validator);
            alarmProcessor = new AlarmProcessor();
            

            try
            {
                LoadFromConfiguration();
                factory = new ChannelFactory<IService>(new NetTcpBinding(), $"net.tcp://localhost:{servicePort}/WCFService");
                proxy = factory.CreateChannel();
                Connected = false;
                UpdateEvent += MainWindowViewModel_UpdateEvent;
                acquisitor = new Acquisitor(Items, acquisitionInterval, factory, commandExecutor, Connected);
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
            acquisitionInterval = config.AquisitionInterval * 1000;
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
        #endregion

        #region Commands & Events

        private void MainWindowViewModel_UpdateEvent(object sender, EventUpdateArgs e)
        {
            grid.Dispatcher.Invoke(() =>
            {
                if (e.Devices != null)
                    Update(e.Devices);
            });
            Connected = e.Connected;
        }

        public void SaveChanges(DataGridCellEditEndingEventArgs args)
        {
            var device = args.EditingElement.DataContext as Device;
            var value = Convert.ToDouble((args.EditingElement as TextBox).Text);
            var index = args.Row.GetIndex();
            
            if(!commandProcessor.ProcessCommand(device, value))
            {
                args.Cancel = true;
                ResetCell(grid, device, index);
                notificationService.ShowNotification("Error", "Cannot command on input devices!", Notifications.Wpf.NotificationType.Error);
            }
        }

     

        public void Load(RoutedEventArgs args)
        {
            grid = ((args.OriginalSource as Grid).Children[0] as DataGrid);
            CheckRowStatus();
            acquisitor.Start(UpdateEvent);
            notificationService.ShowNotification("Client", $"Started", Notifications.Wpf.NotificationType.Success);
        }

        #endregion

        #region Update
        private void ResetCell(DataGrid grid, Device device, int index)
        {
            var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(device);
            (grid.Columns[5].GetCellContent(row) as TextBox).Background = row.Background;
            (grid.Columns[5].GetCellContent(row) as TextBox).Text = (Items[index] as AnalogInput).Value.ToString();
        }

        public void CheckRowStatus()
        {
            foreach (var item in Items)
            {
                item.Alarm = alarmProcessor.ProcessDevice(item);
                var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromItem(item);
                if (item.Alarm.Equals(AlarmType.NO_ALARM))
                    row.Background = Brushes.MediumSeaGreen;
                else
                    row.Background = Brushes.IndianRed;
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
        public void Close(object args)
        {
            acquisitor.Dispose();
        }
    }
}
