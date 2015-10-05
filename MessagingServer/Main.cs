using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessagingServer
{
    public partial class Main : Form
    {
        private TextWriter writer;
        private Server server;
        private Thread serverThread;

        public Main()
        {
            InitializeComponent();
            this.writer = new TextBoxStreamWriter(this.tbConsole);
            Console.SetOut(writer);
            Console.WriteLine("Redirecting messages...");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Starting server...");
            try
            {

                this.server = new Server(Int32.Parse(nudPort.Value.ToString()));
                this.serverThread = new Thread(server.RequestStart);
                this.serverThread.Start();
                this.handleGUIElements();
                Console.WriteLine(String.Format("Listening TCP port {0}", this.server.Port));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void handleGUIElements()
        {
            this.btnStart.Enabled = !this.btnStart.Enabled;
            this.nudPort.Enabled = !this.nudPort.Enabled;
            this.btnStop.Enabled = !this.btnStop.Enabled;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.server.RequestStop();
            this.serverThread.Join();
            this.handleGUIElements();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.server.SendMessage("Ola mundo!");
        }
    }
}
