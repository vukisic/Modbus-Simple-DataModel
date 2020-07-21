using Common.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class Acquisitor : IDisposable
    {
        private Thread acquisitor;
        private event EventHandler<EventUpdateArgs> update;

        private bool connected;
        private CommandExecutor executor;
        private ChannelFactory<IService> factory;
        private IService proxy;
        private int timeout;
        private ObservableCollection<Device> devices;
        private bool stop = false;

        public Acquisitor(ObservableCollection<Device> devices, int timeout, ChannelFactory<IService> factory, CommandExecutor executor, bool connected)
        {
            this.devices = devices;
            this.timeout = timeout;
            this.factory = factory;
            this.executor = executor;
            this.connected = connected;
        }

        public void Start(EventHandler<EventUpdateArgs> update)
        {
            this.update = update;
            stop = false;
            proxy = factory.CreateChannel();
            acquisitor = new Thread(AcquisitorLogic);
            acquisitor.Start();
        }

        public void Stop()
        {
            stop = true;
            acquisitor.Abort();
            acquisitor = null;
            proxy = null;
        }

        private void AcquisitorLogic()
        {
            while (!stop)
            {

                DoWork();
                Thread.Sleep(timeout);
            }
        }

        private void DoWork()
        {
            try
            {
                var devices = proxy.GetAllDevices();
                executor.ExecuteCommands(proxy);
                devices = proxy.GetAllDevices();
                connected = true;
                update?.Invoke(this, new EventUpdateArgs() { Devices = devices, Connected = connected});
            }
            catch (Exception)
            {

                connected = false;
                proxy = factory.CreateChannel();
                update?.Invoke(this, new EventUpdateArgs() { Devices = null, Connected = connected });
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
