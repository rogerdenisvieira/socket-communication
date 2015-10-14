using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private int _port;
        private Socket _serverSocket;
        private BindingList<Socket> _clientSockets = new BindingList<Socket>();
        private byte[] _buffer = new byte[1024];
        public event EventHandler<MessageEventArgs> RaiseMessage;

        #region Construtor

        public Server()
        {
        }

        #endregion

        #region Setup do Servidor
        public void SetupServer(int port)
        {
            _port = port;
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }
        #endregion

        #region Comunicação

        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {

                Socket socket = _serverSocket.EndAccept(AR);
                _clientSockets.Add(socket);
                OnRaiseMessage(new MessageEventArgs(String.Format("New connection from {0}", socket.RemoteEndPoint.ToString())));
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception ex)
            {
                OnRaiseMessage(new MessageEventArgs("Server is not running."));
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = (Socket)AR.AsyncState;
                int received = socket.EndReceive(AR);

                if (received == 0)
                {
                    OnRaiseMessage(new MessageEventArgs("Client has disconnected."));
                }


                byte[] dataBuffer = new byte[received];
                Array.Copy(_buffer, dataBuffer, received);

                string text = Encoding.ASCII.GetString(dataBuffer);

                string response = string.Empty;

                if (text.ToLower() != "get time")
                {
                    response = "Invalid Request";
                }
                else
                {
                    response = DateTime.Now.ToLongTimeString();
                }

                byte[] data = Encoding.ASCII.GetBytes(response);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch (SocketException ex)
            {
                OnRaiseMessage(new MessageEventArgs(ex.Message));
            }
        }

        private void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
            OnRaiseMessage(new MessageEventArgs(String.Format("Response sent to host:  {0}", socket.RemoteEndPoint.ToString())));
            
        }

#endregion

        #region Eventos

        public void OnRaiseMessage(MessageEventArgs args)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessage;
            handler(this, args);
        }
        #endregion

        public void RequestToStop()
        {
            foreach (Socket sck in _clientSockets)
            {
                try
                {
                    OnRaiseMessage(new MessageEventArgs(String.Format("Trying to disconnect from {0}", sck.RemoteEndPoint)));
                    sck.Disconnect(true);
                }
                catch (Exception)
                {
                    OnRaiseMessage(new MessageEventArgs("An error has benn occurred"));
                }
            }

            _serverSocket.Close();
            _serverSocket.Dispose();
        }

        //private string ProcessMessages(string message)
        //{
        //    switch (message)
        //    {
        //        default:
        //            break;
        //    }
        //}

        #region Propriedades

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public BindingList<Socket> ClientSockets
        {
            get { return _clientSockets; }
            set { _clientSockets = value; }
        }

        #endregion

    }
}
