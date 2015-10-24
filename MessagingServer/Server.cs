using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

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

        //aceita as conexões recebidas
        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = _serverSocket.EndAccept(AR);
                _clientSockets.Add(socket);
                OnRaiseMessage(new MessageEventArgs(String.Format("New connection from {0}", socket.RemoteEndPoint.ToString())));

                //inicia a escuta de solicitações pendentes
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception ex)
            {
                OnRaiseMessage(new MessageEventArgs("Server is not running."));
            }
        }


        //recebe as mensagens da conexão aceita
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

                    //trata o protocolo de chegada
                    ProcessIncomingMessages(receivedMessage, socket);

                    // inicia o recebimento novamente
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                }
                catch (SocketException ex)
                {
                    OnRaiseMessage(new MessageEventArgs(ex.Message));
                }
            }
        }


        //envia a mensagem para todos os usuários conextados
        private void BroadcastMessage(String message, Socket socket)
        {
            OnRaiseMessage(new MessageEventArgs("Trying to send message to all connected clients..."));

            // converte a mensagem de resposta em uma cadeia de bytes
            byte[] responseData = Encoding.ASCII.GetBytes(message);

            // envia mensagem para todos os sockets conectados
            foreach (Socket sck in _clientSockets)
            {
                if (sck.Connected && !sck.Equals(socket))
                {
                    // envia a cadeia de bytes de  resposta
                    sck.BeginSend(responseData, 0, responseData.Length, SocketFlags.None, new AsyncCallback(SendCallback), sck);
                }
            }
        }

        //finaliza um envio pendente
        private void SendCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            socket.EndSend(AR);
            OnRaiseMessage(new MessageEventArgs(String.Format("Response sent to host:  {0}", socket.RemoteEndPoint.ToString())));

        }

        #endregion

        #region Eventos

        //delega ao método o evento de chegada de mensagens
        public void OnRaiseMessage(MessageEventArgs args)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessage;
            handler(this, args);
        }
        #endregion

        #region Desconexão
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
                catch (Exception ex)
                {
                    OnRaiseMessage(new MessageEventArgs("An error has been occurred." + ex.Message));
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
        #endregion

        #region Tratamento de protocolo
        private void ProcessIncomingMessages(string message, Socket socket)
        {

            if (message.Substring(0,5) == "[MSG]")
            {
                //retira o cabeçalho e envia a mensagem
                BroadcastMessage(message.Substring(5), socket);
            }

            else if (message.Substring(0,5) == "[LGN]")
            {
                //retira o cabeçalho e faz o login
                DoLogin(message.Substring(5), socket);
            }
        }

        private void DoLogin(string usrAndPass, Socket socket)
        {
            //separa o nome e a senha
            string[] args = usrAndPass.Split(':');

            if(Utils.RemoveBrackets(args[0]) != "rogervieira" || Utils.RemoveBrackets(args[1]) != "mandolate")
            {
                byte[] data = Encoding.ASCII.GetBytes("[NOK]");
                OnRaiseMessage(new MessageEventArgs(String.Format("Connection refused to host {0}", socket.RemoteEndPoint.ToString())));
                _clientSockets.Remove(socket);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
            }
        }
        #endregion

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
