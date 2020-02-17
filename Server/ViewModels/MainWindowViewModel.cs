using Caliburn.Micro;
using Common.ConfigurationTools;
using Common.Devices;
using Server.Models;
using Server.Services;
using Server.Validators;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static Server.Core.MyCommand;

namespace Server.ViewModels
{
    public class MainWindowViewModel : Conductor<object>, INotifyPropertyChanged
    {
        #region Properties
        private BindingList<DeviceModel> items;
        public BindingList<DeviceModel> Items { get { return items; } set { items = value; OnPropertyChanged("Items"); } }

        private IDeviceValidator validator;
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

        public MainWindowViewModel(IDeviceValidator validator, INotificationService notification, IDataRepository repo)
        {
            this.validator = validator;
            this.notificationService = notification;
            this.repository = repo;
            StartCommand = new MyICommand(OnStart);
            StopCommand = new MyICommand(OnStop);
            Items = new BindingList<DeviceModel>();
            
            SimulationStatus = "InActive";
            try
            {
                LoadFromConfiguration();
            }
            catch (Exception)
            {

                notificationService.ShowNotification("Server", "Error occured while reading configuration", Notifications.Wpf.NotificationType.Error);
            }
            
        }

        #region Helpers
        public void LoadFromConfiguration()
        {
            ConfigurationParser parser = new ConfigurationParser("../../../confguration.json");
            var config = parser.Read();
            var converter = new ConfigurationToModelsConverter();
            var devs = converter.ConvertToDevices(config);

            repository.RemoveAll();
            foreach (var item in devs.DigitalOutputs)
            {
                Items.Add(item);
                repository.Add(item);
            }
            foreach (var item in devs.DigitalInputs)
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
                if (validator.Validate(item as DeviceModel))
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

            if (device is DigitalInputModel)
            {
                repository.Update(Items[index] as DigitalInputModel, (byte)value);
            }
            else if (device is DigitalOutputModel)
            {
                repository.Update(Items[index] as DigitalOutputModel, (byte)value);
            }
            else if (device is AnalogOutputModel)
            {
                repository.Update(Items[index] as AnalogOutputModel, value);
            }
            else if (device is AnalogInputModel)
            {
                repository.Update(Items[index] as AnalogInputModel, value);
            }

            if (validator.Validate(device as DeviceModel, value))
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
        }

        public void Close(object args)
        {
            endSignal = false;
            updateThread.Abort();
            updateThread = null;
            simulationThread.Abort();
            simulationThread = null;
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
                    if (item is DigitalInputModel)
                    { 
                        var itemToUpdate = Items.Single(x => x.Address == item.Address) as DigitalInputModel;
                        var newItem = item as DigitalInputModel;
                        if (itemToUpdate.Value != newItem.Value)
                            itemToUpdate.Value = newItem.Value;
                    }
                    else if (item is DigitalOutputModel)
                    {
                        var itemToUpdate = Items.Single(x => x.Address == item.Address) as DigitalOutputModel;
                        var newItem = item as DigitalOutputModel;
                        if (itemToUpdate.Value != newItem.Value)
                            itemToUpdate.Value = newItem.Value;
                    }
                    else if (item is AnalogInputModel)
                    {
                        var itemToUpdate = Items.Single(x => x.Address == item.Address) as AnalogInputModel;
                        var newItem = item as AnalogInputModel;
                        if (itemToUpdate.Value != newItem.Value)
                            itemToUpdate.Value = newItem.Value;
                    }
                    else if (item is AnalogOutputModel)
                    {
                        var itemToUpdate = Items.Single(x => x.Address == item.Address) as AnalogOutputModel;
                        var newItem = item as AnalogOutputModel;
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
                if (item is DigitalInputModel)
                {
                    var value = (byte)(rand.Next(0, 10) % 2);
                    repository.Update((item as DigitalInputModel), value);
                }
                else if (item is DigitalOutputModel)
                {
                    var value = (byte)(rand.Next(0, 10) % 2);
                    repository.Update((item as DigitalOutputModel), value);
                }
                else if (item is AnalogInputModel)
                {
                    var device = item as AnalogInputModel;
                    var lowerBound = (int)(device.MinValue) + 1000;
                    var upperBound = (int)(device.MaxValue) + 1000;
                    repository.Update((item as AnalogInputModel), (double)(rand.Next(lowerBound, upperBound)));
                }
                else if (item is AnalogOutputModel)
                {
                    var device = item as AnalogOutputModel;
                    var lowerBound = (int)(device.MinValue) + 1000;
                    var upperBound = (int)(device.MaxValue) + 1000;
                    repository.Update((item as AnalogOutputModel), (double)(rand.Next(lowerBound, upperBound)));
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
