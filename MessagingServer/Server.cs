using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private static string _responseMessage;
        private bool _isRunning = false;

        #region Construtor

        public Server()
        {
        }

        #endregion

        #region Setup do Servidor
        public void SetupServer(int port)
        {
            _isRunning = true;
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
            if (_isRunning)
            {
                try
                {
                    Socket socket = (Socket)AR.AsyncState;
                    int receivedSize = socket.EndReceive(AR);

                    //checa se o cliente desconectou-se
                    if (receivedSize == 0)
                    {
                        OnRaiseMessage(new MessageEventArgs("Client has been disconnected from the server."));
                        socket.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), socket);
                        return;
                    }

                    // converte a cadeia de bytes recebida em uma mensagem
                    byte[] receivedData = new byte[receivedSize];
                    Array.Copy(_buffer, receivedData, receivedSize);
                    string receivedMessage = Encoding.ASCII.GetString(receivedData);

                    ProcessMessages(receivedMessage);

                    // inicia o recebimento novamente
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                }
                catch (SocketException ex)
                {
                    OnRaiseMessage(new MessageEventArgs(ex.Message));
                }
            }
        }

        private void BroadcastMessage(String message)
        {
            OnRaiseMessage(new MessageEventArgs("Trying to send message to all connected clients..."));

            // converte a mensagem de resposta em uma cadeia de bytes
            byte[] responseData = Encoding.ASCII.GetBytes(message);

            // envia mensagem para todos os sockets conectados
            foreach (Socket sck in _clientSockets)
            {
                if (sck.Connected)
                {
                    // envia a cadeia de bytes de  resposta
                    sck.BeginSend(responseData, 0, responseData.Length, SocketFlags.None, new AsyncCallback(SendCallback), sck);
                }
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

        // TODO: corrigir a desconexão
        public void RequestToStop()
        {
            _isRunning = false;
            foreach (Socket sck in _clientSockets)
            {
                try
                {
                    OnRaiseMessage(new MessageEventArgs(String.Format("Trying to disconnect from {0}", sck.RemoteEndPoint)));
                    sck.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), sck);
                }
                catch (Exception)
                {
                    OnRaiseMessage(new MessageEventArgs("An error has been occurred"));
                }
            }

            _serverSocket.Close();

        }

        private void DisconnectCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            if (socket.Connected)
            {
                _clientSockets.Remove(socket);
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(true);
                socket.Close();
            }
            else
            {
                socket.Close();
            }
        }

        private void ProcessMessages(string message)
        {
            Debug.Write(message + "\n");
            Debug.Write(message.Substring(0, 4));

            if (message.Substring(0,5) == "[MSG]")
            {
                BroadcastMessage(message.Substring(5));
            }

            else if (message.Substring(0,4) == "[LGN]")
            {
                DoLogin(message.Substring(0,5));
            }
        }

        private void DoLogin(string args)
        {
            
        }

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

        public static string ResponseMessage
        {
            get { return _responseMessage; }
            set { _responseMessage = value; }
        }

        #endregion

    }
}
