using DesktopClient;
using MessagingServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TesteComunicaçãoSocket
{
    public partial class ClientMain : Form
    {
        private BindingList<Mensagem> _messages;
        private TextWriter _writer;
        private Client _client;

        public ClientMain()
        {
            InitializeComponent();
            _messages = new BindingList<Mensagem>();
            _writer = new TextBoxStreamWriter(this.tbOutput);
            Console.SetOut(this._writer);
            _client = new Client();
            _client.RaiseMessage += HandleMessageEvent;

        }

        //refatorar
        private void button1_Click(object sender, EventArgs e)
        {
            if (tbInput.Text != string.Empty)
            {
                SendMessage(tbInput.Text);
            }
        }

        //refatorar
        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && tbInput.Text != string.Empty)
            {

                SendMessage(tbInput.Text);
            }
        }

        public void HandleMessageEvent(object sender, MessageEventArgs e)
        {
            _messages.Add(new Mensagem
            {
                TextoMensagem = e.Message,
                TimeStamp = System.DateTime.Now,
            });
            Console.WriteLine(String.Format("{0} : {1}", _messages.Last().TimeStamp.ToString(), _messages.Last().TextoMensagem));
        }

        public void SendMessage(string message) 
        {
            _messages.Add(new Mensagem
            {
                TextoMensagem = this.tbInput.Text,
                TimeStamp = System.DateTime.Now,
            });

            Console.WriteLine(FormatLogMessage(_messages.Last().TextoMensagem));
            _client.SendMessage(_messages.Last().TextoMensagem);
            this.tbInput.Clear();      
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            IPAddress ip;
            int port;

            if (IPAddress.TryParse(this.tbServerIP.Text, out ip) && Int32.TryParse(this.nudPort.Value.ToString(), out port))
            {                
                _client.Connect(port, ip);
                handleGUIElements();
            }
            else
            {
                Console.WriteLine(FormatLogMessage("Invalid TCP Port or IP Address."));
            }


        }

        private void handleGUIElements()
        {
            this.btnStart.Enabled = !this.btnStart.Enabled;
            this.nudPort.Enabled = !this.nudPort.Enabled;
            this.btnStop.Enabled = !this.btnStop.Enabled;
            this.tbServerIP.Enabled = !this.tbServerIP.Enabled;
            this.tbInput.Enabled = !this.tbInput.Enabled;
            this.btnSend.Enabled = !this.btnSend.Enabled;
        }

        // TODO implementar a desconexão
        private void btnStop_Click(object sender, EventArgs e)
        {
            _client.Disconnect();
            handleGUIElements();
        }

        private string FormatLogMessage(string message)
        {
            return String.Format("{0} : {1}", System.DateTime.Now, message);
        }
    }
}
