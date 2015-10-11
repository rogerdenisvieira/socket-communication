using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingServer
{
    public class MessageEventArgs
    {
        private string _message;

        public MessageEventArgs(string msg)
        {
            _message = msg;
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
