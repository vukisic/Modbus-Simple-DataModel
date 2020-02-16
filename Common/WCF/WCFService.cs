using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.WCF
{
    /// <summary>
    /// WCFService represent implementation of WCF Service
    /// </summary>
    public class WCFService : IDisposable
    {
        private ServiceHost host;
        private string name;
        private IPEndPoint endPoint;
        private Type serviceType;
        private Type contractType;

        /// <summary>
        /// Constructs new WCF Service
        /// </summary>
        /// <param name="name">Name of service</param>
        /// <param name="endPoint">IPEndPoint for service</param>
        /// <param name="serviceType">Service type</param>
        /// <param name="contractType">Contract type</param>
        public WCFService(string name, IPEndPoint endPoint, Type serviceType, Type contractType)
        {
            this.name = name;
            this.endPoint = endPoint;
            this.serviceType = serviceType;
            this.contractType = contractType;
        }

        /// <summary>
        /// Creates WCF Service
        /// </summary>
        public void Create()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = $"net.tcp://{endPoint}/{name}";
            host = new ServiceHost(serviceType);
            host.AddServiceEndpoint(contractType, binding, new Uri(address));
        }

        /// <summary>
        /// Opens WCF Service
        /// </summary>
        public void Open()
        {
            try
            {
                if (host == null || (host.State != CommunicationState.Opened && host.State != CommunicationState.Opening))
                {
                    if (host == null)
                        Create();
                    host.Open();
                }


            }
            catch (Exception)
            {

                throw new Exception("Service Host failed to open!");
            }
        }

        /// <summary>
        /// Closes WCF Service
        /// </summary>
        public void Close()
        {
            try
            {
                if (host.State != CommunicationState.Closed && host.State != CommunicationState.Closing)
                {
                    host.Close();
                    host = null;
                }

            }
            catch (Exception)
            {

                throw new Exception("Service Host failed to close!");
            }
        }

        public void Dispose()
        {
            host = null;
        }
    }
}
