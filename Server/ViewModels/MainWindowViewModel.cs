using Caliburn.Micro;
using Common.ConfigurationTools;
using Common.Devices;
using Server.Models;
using Server.Services;
using Server.Validators;
using System;
using System.ComponentModel;
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
        private DataGrid grid;

        
        private Thread simulationThread;
        private string simulationStatus;
        private bool simulation;
        public bool Simulation { get { return simulation; } set { simulation = value; OnPropertyChanged("Simulation"); } }
        public string SimulationStatus { get { return simulationStatus; } set { simulationStatus = value; OnPropertyChanged("SimulationStatus"); } }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public MyICommand StartCommand { get; set; }
        public MyICommand StopCommand { get; set; }

        #endregion

        public MainWindowViewModel(IDeviceValidator validator, INotificationService notification)
        {
            this.validator = validator;
            this.notificationService = notification;
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


            foreach (var item in devs.DigitalOutputs)
            {
                Items.Add(item);
            }
            foreach (var item in devs.DigitalInputs)
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
            var value = Convert.ToInt32((args.EditingElement as TextBox).Text);
            var index = args.Row.GetIndex();

            if (device is DigitalInput)
            {
                (Items[index] as DigitalInputModel).Value = (byte)value;

            }
            if (device is DigitalOutput)
            {
                (Items[index] as DigitalOutputModel).Value = (byte)value;
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
        }
        #endregion

        #region Thread
        public void ThreadFunction()
        {
            Random rand = new Random();
            SimulationStatus = "Active";
            while (Simulation)
            {

                SimulationLogic();
                grid.Dispatcher.Invoke(() =>
                {
                    CheckRowStatus();
                });
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
                    (item as DigitalInputModel).Value = (byte)(rand.Next(0, 10) % 2);
                }
                else if (item is DigitalOutputModel)
                {
                    (item as DigitalOutputModel).Value = (byte)(rand.Next(0, 10) % 2);
                }
                else if (item is AnalogInputModel)
                {
                    var device = item as AnalogInputModel;
                    var lowerBound = (int)(device.MinValue) + 1000;
                    var upperBound = (int)(device.MaxValue) + 1000;
                    (item as AnalogInputModel).Value = (double)(rand.Next(lowerBound, upperBound));
                }
                else if (item is AnalogOutputModel)
                {
                    var device = item as AnalogOutputModel;
                    var lowerBound = (int)(device.MinValue) + 1000;
                    var upperBound = (int)(device.MaxValue) + 1000;
                    (item as AnalogOutputModel).Value = (double)(rand.Next(lowerBound, upperBound));
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
