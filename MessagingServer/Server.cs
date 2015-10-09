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
        private int _port;
        private Socket _serverSocket;
        private List<Socket> _clientSockets;
        private byte[] _buffer;


        

        public Server(int aPort)
        {
            _port = aPort;
            _clientSockets = new List<Socket>();
            _buffer = new byte[1024];
            _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void SetupServer()
        {
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            System.Windows.Forms.MessageBox.Show("client connected...");
            Socket socket = _serverSocket.EndAccept(AR);
            _clientSockets.Add(socket);
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
            //Console.WriteLine("Text received: " + text);
            System.Windows.Forms.MessageBox.Show(text);

        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

    }
}
