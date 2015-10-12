using DesktopClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TesteComunicaçãoSocket
{
    class Client
    {
        public event EventHandler<MessageEventArgs> RaiseMessage;
        private Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void Connect()
        {
            int attempts = 0;

            while (!_clientSocket.Connected)
            {
                try
                {                  
                    _clientSocket.Connect(IPAddress.Loopback, 1025);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void SendMessage(string message)
        {
            byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            _clientSocket.Send(sendBuffer);
            
            byte[] receiveBuffer = new byte[1024];
            int rec = _clientSocket.Receive(receiveBuffer);

            byte[] data = new byte[rec];
            Array.Copy(receiveBuffer, data, rec);

            OnRaiseMessage(new MessageEventArgs(Encoding.ASCII.GetString(data)));
        }

        public void OnRaiseMessage(MessageEventArgs args)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessage;
            handler(this, args);
        }
    }
}
