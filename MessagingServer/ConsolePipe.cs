using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessagingServer
{
    class ConsolePipe
    {
        public static void Print(TextBox tb, string msg)
        {
            tb.Text = msg;
        }
    }
}
