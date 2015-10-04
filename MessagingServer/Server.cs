using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessagingServer
{
    class Server
    {
        public int Port { get; set; }
        private List<StreamWriter> writer;
        public Socket Client { get; set; }

        public Server(int aPort)
        {
            this.Port = aPort;
            this.writer = new List<StreamWriter>();

        }

        public void execute()
        {
            TcpListener listener = new TcpListener(this.Port);
            listener.Start();
            this.Client = listener.AcceptSocket();            
        }

    }
}
