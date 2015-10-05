using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessagingServer
{
    class Server
    {
        public int Port { get; set; }
        private TcpListener tcpListener;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        

        public Server(int aPort)
        {
            this.Port = aPort;
        }

        private void Connect()
        {
            try
            {
                this.tcpListener = new TcpListener(IPAddress.Any, this.Port);
                this.tcpListener.Start();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Disconnect()
        {
            try
            {
                if (this.tcpClient != null)
                {
                    this.tcpClient.Client.Disconnect(true);
                }
                this.tcpListener.Stop();
                Console.WriteLine("Disconnected");
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("Error: {0}", ex.Message));
            }
        }

        private void AcceptConnection()
        {
            try
            {
                this.tcpClient = this.tcpListener.AcceptTcpClient();
                this.networkStream = this.tcpClient.GetStream();
                
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void SendMessage(String message)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(message);
            this.networkStream.Write(buffer, 0, buffer.Length);
        }




        public void RequestStart()
        {
            Connect();
            AcceptConnection();
        }

        public void RequestStop()
        {
            this.Disconnect();
        }



    }
}
