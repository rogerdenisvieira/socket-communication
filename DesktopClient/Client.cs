using DesktopClient;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TesteComunicaçãoSocket
{
    class Client
    {
        public event EventHandler<MessageEventArgs> RaiseMessage;
        private Socket _clientSocket;
        private byte[] _buffer = new byte[1024];
        private String _nick;

        #region Conexão
        public void Connect(int aPort, IPAddress Ip, string userName, string password, string nick)
        {
            int attempts = 5;
            _nick = nick;

                
            // TODO: validar esse objeto, pois após a desconexão ocorre o dispose() dele
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            while (!_clientSocket.Connected && attempts > 0)
            {
                try
                {
                    OnRaiseMessage(new MessageEventArgs(String.Format("Attempt {0} to connect at server {1}:{2}", attempts, Ip.MapToIPv4().Address, aPort)));                    
                    _clientSocket.Connect(Ip ,aPort);
                    OnRaiseMessage(new MessageEventArgs("Connected!"));

                    //solicita autenticação
                    Authenticate(userName, password);

                    //inicia recebimento assíncrono de mensagens
                    _clientSocket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), _clientSocket);


                }
                catch (Exception ex)
                {
                    OnRaiseMessage(new MessageEventArgs("Connection failed."));
                    Thread.Sleep(1000);         
                    attempts--;
                }
            }
        }
        #endregion

        #region Comunicação
        //trata as mensagens recebidas assincronamente
        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = (Socket)AR.AsyncState;
                int receivedSize = socket.EndReceive(AR);

                //verifica se o servidor não desconectou-se
                if (receivedSize == 0)
                {
                    OnRaiseMessage(new MessageEventArgs("Cannot connect to remote server."));
                    socket.Shutdown(SocketShutdown.Both);
                    return;
                }

                // converte a cadeia de bytes recebida em uma mensagem
                byte[] receivedData = new byte[receivedSize];
                Array.Copy(_buffer, receivedData, receivedSize);
                string receivedMessage = Encoding.ASCII.GetString(receivedData);


                //checa a autenticação
                if(receivedMessage.Substring(0,5) == "[NOK]")
                {
                    OnRaiseMessage(new MessageEventArgs("Authentication failed!"));
                    socket.Disconnect(false);
                    return;
                }
                else
                {
                    //envia mensagens se estiver autenticado
                    OnRaiseMessage(new MessageEventArgs(receivedMessage));
                }

                // inicia o recebimento novamente
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch (SocketException ex)
            {
                OnRaiseMessage(new MessageEventArgs(ex.Message));
                throw;
            }
        }

        //envia mensagens
        public void SendMessage(string message)
        {
            byte[] sendBuffer = Encoding.ASCII.GetBytes(String.Format("[MSG]{0} says: {1}", _nick, message));
            _clientSocket.Send(sendBuffer);
            
            OnRaiseMessage(new MessageEventArgs(message));
        }
        #endregion

        #region Protocolo

        //envia as informações para autenticação
        public void Authenticate(string userName, string password)
        {
            byte[] sendBuffer = Encoding.ASCII.GetBytes(String.Format("[LGN][{0}]:[{1}]", userName, password));
            _clientSocket.Send(sendBuffer);
        }
        #endregion

        #region Eventos
        public void OnRaiseMessage(MessageEventArgs args)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessage;
            handler(this, args);
        }
        #endregion
    }
}
