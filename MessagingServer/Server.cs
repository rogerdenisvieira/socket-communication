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
        private Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private BindingList<Socket> _clientSockets = new BindingList<Socket>();
        private byte[] _buffer = new byte[1024];
        public event EventHandler<MessageEventArgs> RaiseMessage;


        public Server()
        {
        }

        public void SetupServer()
        {
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void AcceptCallback(IAsyncResult AR)
        {

            Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
            OnRaiseMessage(new MessageEventArgs(String.Format("New connection from {0}", socket.RemoteEndPoint.ToString())));
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            int received = socket.EndReceive(AR);
            byte[] dataBuffer = new byte[received];
            Array.Copy(_buffer, dataBuffer, received);

            string text = Encoding.ASCII.GetString(dataBuffer);

            string response = string.Empty;

            if (text.ToLower() != "t")
            {
                response = "Invalid Request";
            }
            else
            {
                response = DateTime.Now.ToLongTimeString();
            }

            byte[] data = Encoding.ASCII.GetBytes(response);
            socket.BeginSend(data,0,data.Length, SocketFlags.None, new AsyncCallback(SendCallback),socket);
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }

        private void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
            OnRaiseMessage(new MessageEventArgs(String.Format("Response sent to host:  {0}", socket.RemoteEndPoint.ToString())));
            
        }

        public void OnRaiseMessage(MessageEventArgs args)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessage;
            handler(this, args);
        }



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

    }
}
