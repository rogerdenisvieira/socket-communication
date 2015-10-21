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
        private Socket _clientSocket;
        private byte[] _buffer = new byte[1024];
        

        public void Connect(int aPort, IPAddress Ip)
        {
            int attempts = 5;

                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            while (!_clientSocket.Connected && attempts > 0)
            {
                try
                {
                    OnRaiseMessage(new MessageEventArgs(String.Format("Attempt {0} to connect at server {1}:{2}", attempts, Ip.MapToIPv4().Address, aPort)));                    
                    _clientSocket.Connect(Ip ,aPort);
                    OnRaiseMessage(new MessageEventArgs("Connected!"));

                    _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _clientSocket);


                }
                catch (Exception ex)
                {
                    OnRaiseMessage(new MessageEventArgs("Failed!"));                    
                    attempts--;
                }
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = (Socket)AR.AsyncState;
                int receivedSize = socket.EndReceive(AR);

                if (receivedSize == 0)
                {
                    OnRaiseMessage(new MessageEventArgs("Server disconnected!"));
                    socket.Shutdown(SocketShutdown.Both);
                    return;
                }

                // converte a cadeia de bytes recebida em uma mensagem
                byte[] receivedData = new byte[receivedSize];
                Array.Copy(_buffer, receivedData, receivedSize);
                string receivedMessage = Encoding.ASCII.GetString(receivedData);

                OnRaiseMessage(new MessageEventArgs(receivedMessage));

                // inicia o recebimento novamente
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch (SocketException ex)
            {
                OnRaiseMessage(new MessageEventArgs(ex.Message));
                throw;
            }
        }

        public void Disconnect()
        {
            if (_clientSocket.Connected)
            {
                try
                {
                    _clientSocket.Shutdown(SocketShutdown.Both);
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
            
            OnRaiseMessage(new MessageEventArgs(message));
        }

        public void OnRaiseMessage(MessageEventArgs args)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessage;
            handler(this, args);
        }
    }
}
