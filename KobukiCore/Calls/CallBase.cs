using KobukiCore.KobukiAssets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KobukiCore.Calls
{
    public abstract class CallBase : IDisposable
    {
        #region Properties
        protected UdpClient udpClient;
        protected IPEndPoint Host;
        protected int SendedDataCount { get; private set; } = 0;
        protected int ReceivedDataCount { get; private set; } = 0;
        public virtual string Hostname { get; } = "127.0.0.1";
        public virtual int PortSend { get; } = 5300;
        public virtual int PortRecieve { get; } = 53000;
        #endregion
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public CallBase()
        {
            udpClient = new UdpClient();
            Host = new IPEndPoint(IPAddress.Parse(Hostname), PortRecieve);
            udpClient.Client.Bind(Host);
        }
        #endregion
        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected bool WriteCommandOnly(byte[] command)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            try
            {
                if ((SendedDataCount = udpClient.Send(command, command.Length, Hostname, PortSend)) != command.Length)
                {
                    SendedDataCount = -1;
                }
            }
            catch (Exception ex)
            {
                SendedDataCount = -1;
            }
            return SendedDataCount == command.Length;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected byte[] ReadStateOnly()
        {
            byte[] result = null;
            try
            {
                if ((result = udpClient.Receive(ref Host)) == null)
                {
                    ReceivedDataCount = -1;
                }
                ReceivedDataCount = result.Length;
            }
            catch (Exception ex)
            {
                ReceivedDataCount = -1;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected byte[] WriteCommandReadState(byte[] command)
        {
            byte[] result = null;
            var host = new IPEndPoint(IPAddress.Parse(Hostname), PortRecieve);
            if (command == null)
                throw new ArgumentNullException("command");
            try
            {
                /*using (var client = new UdpClient())
                {
                    var data = 0;
                    if ((data = client.Send(command, command.Length,Hostname,PortSend)) == -1 || data != command.Length)
                    {
                        throw new ArgumentException("Data were not correctly send!");
                    }
                    if ((result = client.Receive(ref host)) == null)
                    {
                        throw new ArgumentException("Data were not correctly received!");
                    }
                }*/
                if ((SendedDataCount = udpClient.Send(command, command.Length, Hostname, PortSend)) != command.Length)
                {
                    SendedDataCount = -1;
                }
                if ((result = udpClient.Receive(ref host)) == null)
                {
                    ReceivedDataCount = -1;
                }
                ReceivedDataCount = result.Length;
            }
            catch (Exception)
            {
                ReceivedDataCount = -1;
                SendedDataCount = -1;
                result = null;
            }
            return result;
        }

        public void Dispose()
        {
            udpClient?.Close();
            udpClient?.Dispose();
        }
        #endregion
    }
}
