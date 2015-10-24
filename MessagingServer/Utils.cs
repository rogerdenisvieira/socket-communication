using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingServer
{
    class Utils
    {
        public static String RemoveBrackets(string s)
        {
            string temp = s.Remove(0, 1);
            return temp = temp.Remove((temp.Length - 1));           
        }
    }
}
