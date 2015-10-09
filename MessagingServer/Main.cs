using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessagingServer
{
    public partial class Main : Form
    {
        private TextWriter writer;
        private Server _server;
        private Thread _serverThread;

        public Main()
        {
            InitializeComponent();
            this.writer = new TextBoxStreamWriter(this.tbConsole);
            Console.SetOut(writer);
            Console.WriteLine("Redirecting messages...");
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(String.Format("Starting server {0}", Dns.GetHostName()));
            _server = new Server(Int32.Parse(nudPort.Value.ToString()));
            _server.SetupServer();

            handleGUIElements();
        }



        private void btnStop_Click(object sender, EventArgs e)
        {

            this.handleGUIElements();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void handleGUIElements()
        {
            this.btnStart.Enabled = !this.btnStart.Enabled;
            this.nudPort.Enabled = !this.nudPort.Enabled;
            this.btnStop.Enabled = !this.btnStop.Enabled;
        }

    }
}
