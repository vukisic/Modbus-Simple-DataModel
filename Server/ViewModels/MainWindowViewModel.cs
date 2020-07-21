using Caliburn.Micro;
using Common.ConfigurationTools;
using Common.Devices;
using Server.Services;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Common.WCF;
using static Server.Core.MyCommand;
using System.Net;
using Server.WCFService;
using Common.Converters;
using Common.Services;
using Common;

namespace Server.ViewModels
{
    public class MainWindowViewModel : Conductor<object>, INotifyPropertyChanged
    {
        #region Properties
        private BindingList<Device> items;
        public BindingList<Device> Items { get { return items; } set { items = value; OnPropertyChanged("Items"); } }

        private int servicePort;
        private Common.WCF.WCFService service;

        private Common.Devices.IDeviceValidator validator;
        private INotificationService notificationService;
        private IDataRepository repository;
        private DataGrid grid;

        
        private Thread simulationThread;
        private Thread updateThread;
        private string simulationStatus;
        private bool simulation;
        private bool endSignal = true;
        public bool Simulation { get { return simulation; } set { simulation = value; OnPropertyChanged("Simulation"); } }
        public string SimulationStatus { get { return simulationStatus; } set { simulationStatus = value; OnPropertyChanged("SimulationStatus"); } }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public MyICommand StartCommand { get; set; }
        public MyICommand StopCommand { get; set; }

        #endregion

        public MainWindowViewModel(Common.Devices.IDeviceValidator validator, INotificationService notification, IDataRepository repo)
        {
            this.validator = validator;
            this.notificationService = notification;
            this.repository = repo;
            StartCommand = new MyICommand(OnStart);
            StopCommand = new MyICommand(OnStop);
            Items = new BindingList<Device>();
            
            SimulationStatus = "InActive";
            try
            {
                LoadFromConfiguration();
                service = new Common.WCF.WCFService("WCFService", new IPEndPoint(IPAddress.Loopback, servicePort), typeof(Service), typeof(IService));
                service.Create();
                service.Open();
               
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
            var converter = new ConfigurationDevicesConverter();
            var devs = converter.ConvertToDevices(config);

            repository.RemoveAll();
            foreach (var item in devs.DigitalInputs)
            {
                Items.Add(item);
                repository.Add(item);
            }
            foreach (var item in devs.DigitalOutputs)
            {
                Items.Add(item);
                repository.Add(item);
            }
            foreach (var item in devs.AnalogInputs)
            {
                Items.Add(item);
                repository.Add(item);
            }
            foreach (var item in devs.AnalogOutputs)
            {
                Items.Add(item);
                repository.Add(item);
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
        public void OnStart()
        {
            Simulation = true;
            simulationThread = new Thread(ThreadFunction);
            simulationThread.Start();

        }

        public void OnStop()
        {
            Simulation = false;
        }

        public void SaveChanges(DataGridCellEditEndingEventArgs args)
        {
            var device = args.EditingElement.DataContext;
            var value = Convert.ToDouble((args.EditingElement as TextBox).Text);
            var index = args.Row.GetIndex();

            if (device is DigitalInput)
            {
                repository.Update(Items[index] as DigitalInput, (byte)value);
            }
            else if (device is DigitalOutput)
            {
                repository.Update(Items[index] as DigitalOutput, (byte)value);
            }
            else if (device is AnalogOutput)
            {
                repository.Update(Items[index] as AnalogOutput, value);
            }
            else if (device is AnalogInput)
            {
                repository.Update(Items[index] as AnalogInput, value);
            }

            if (validator.Validate(device as Device, value))
                args.Row.Background = Brushes.MediumSeaGreen;
            else
                args.Row.Background = Brushes.IndianRed;
        }

        public void Load(RoutedEventArgs args)
        {
            grid = ((args.OriginalSource as Grid).Children[0] as DataGrid);
            CheckRowStatus();
            updateThread = new Thread(Updater);
            updateThread.Start();
            notificationService.ShowNotification("Server", $"Started", Notifications.Wpf.NotificationType.Success);
        }

        public void Close(object args)
        {
            endSignal = false;
            updateThread.Abort();
            updateThread = null;
            if(simulationThread != null)
            {
                simulationThread.Abort();
                simulationThread = null;
            }
               
            service.Close();
            service.Dispose();
            service = null;
        }
        #endregion

        #region Threads
        public void Updater()
        {
            while(endSignal)
            {
                var res = repository.GetAllDeviceBindings();
                foreach (var item in res)
                {
                    if (item is DigitalInput)
                    { 
                        var itemToUpdate = Items.Single(x => x.Address == item.Address) as DigitalInput;
                        var newItem = item as DigitalInput;
                        if (itemToUpdate.Value != newItem.Value)
                            itemToUpdate.Value = newItem.Value;
                    }
                    else if (item is DigitalOutput)
                    {
                        var itemToUpdate = Items.Single(x => x.Address == item.Address) as DigitalOutput;
                        var newItem = item as DigitalOutput;
                        if (itemToUpdate.Value != newItem.Value)
                            itemToUpdate.Value = newItem.Value;
                    }
                    else if (item is AnalogInput)
                    {
                        var itemToUpdate = Items.Single(x => x.Address == item.Address) as AnalogInput;
                        var newItem = item as AnalogInput;
                        if (itemToUpdate.Value != newItem.Value)
                            itemToUpdate.Value = newItem.Value;
                    }
                    else if (item is AnalogOutput)
                    {
                        var itemToUpdate = Items.Single(x => x.Address == item.Address) as AnalogOutput;
                        var newItem = item as AnalogOutput;
                        if (itemToUpdate.Value != newItem.Value)
                            itemToUpdate.Value = newItem.Value;
                    }
                }

                grid.Dispatcher.Invoke(() =>
                {
                    CheckRowStatus();
                });
                if (!endSignal)
                    break;
                Thread.Sleep(1000);
            }
            
        }

        public void ThreadFunction()
        {
            Random rand = new Random();
            SimulationStatus = "Active";
            while (Simulation)
            {

                SimulationLogic();
                if (!Simulation)
                    break;
                Thread.Sleep(rand.Next(1000, 5000));
            }
            SimulationStatus = "InActive";
        }

        public void SimulationLogic()
        {
            Random rand = new Random();
            foreach (var item in Items)
            {
                if (item is DigitalInput)
                {
                    var value = (byte)(rand.Next(0, 10) % 2);
                    repository.Update((item as DigitalInput), value);
                }
                else if (item is DigitalOutput)
                {
                    var value = (byte)(rand.Next(0, 10) % 2);
                    repository.Update((item as DigitalOutput), value);
                }
                else if (item is AnalogInput)
                {
                    var device = item as AnalogInput;
                    var lowerBound = (int)(device.MinValue) + 1000;
                    var upperBound = (int)(device.MaxValue) + 1000;
                    repository.Update((item as AnalogInput), (double)(rand.Next(lowerBound, upperBound)));
                }
                else if (item is AnalogOutput)
                {
                    var device = item as AnalogOutput;
                    var lowerBound = (int)(device.MinValue) + 1000;
                    var upperBound = (int)(device.MaxValue) + 1000;
                    repository.Update((item as AnalogOutput), (double)(rand.Next(lowerBound, upperBound)));
                }
            }
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
