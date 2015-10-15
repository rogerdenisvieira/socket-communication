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


        public void Connect(int aPort, IPAddress Ip)
        {
            int attempts = 5;



            while (!_clientSocket.Connected && attempts > 0)
            {
                try
                {
                    OnRaiseMessage(new MessageEventArgs(String.Format("Attempt {0} to connect at server {1}:{2}", attempts, Ip.MapToIPv4().Address, aPort)));                    
                    _clientSocket.Connect(Ip ,aPort);
                    OnRaiseMessage(new MessageEventArgs("Connected!"));
                    

                }
                catch (Exception ex)
                {
                    OnRaiseMessage(new MessageEventArgs("Failed!"));
                    OnRaiseMessage(new MessageEventArgs(ex.Message));
                    attempts--;
                }
            }
        }

        public void Disconnect()
        {
            if (_clientSocket.Connected)
            {
                try
                {
                    //_clientSocket.Shutdown(SocketShutdown.Both);
                    _clientSocket.Disconnect(true);
                    _clientSocket.Close();
                }
                catch (SocketException ex) 
                {
                    OnRaiseMessage(new MessageEventArgs(ex.Message));
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
