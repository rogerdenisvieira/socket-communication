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
            int result;
            if (int.TryParse(this.nudPort.Value.ToString(), out result))
            {
                this.server = new Server(result);
            }
            else
            {
                Console.WriteLine("Invalid port.");
            }


            try
            {
                this.serverThread = new Thread(server.execute);
                this.serverThread.Start();
                this.handleGUIElements();
                Console.WriteLine(String.Format("Port {0} opened.", this.server.Port));
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
            if (this.serverThread.IsAlive)
            {
                this.serverThread.Join();
            }
        }
    }
}
