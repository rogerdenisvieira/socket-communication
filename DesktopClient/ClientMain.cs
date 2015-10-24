using DesktopClient;
using MessagingServer;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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

        //envia as mensagens através de clique
        private void btSend_Click(object sender, EventArgs e)
        {
            if (tbInput.Text != string.Empty)
            {
                SendMessage(tbInput.Text);
            }
        }

        //envia as mensagens com enter
        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && tbInput.Text != string.Empty)
            {
                SendMessage(tbInput.Text);
            }
        }

        //metodo delegado para tratar o evento de chegada das mensagens
        public void HandleMessageEvent(object sender, MessageEventArgs e)
        {
            _messages.Add(new Mensagem
            {
                TextoMensagem = e.Message,
                TimeStamp = System.DateTime.Now,
            });
            Console.WriteLine(FormatLogMessage(_messages.Last().TextoMensagem));
        }


        //envia a mensagem
        public void SendMessage(string message) 
        {
            _messages.Add(new Mensagem
            {
                TextoMensagem = this.tbInput.Text,
                TimeStamp = System.DateTime.Now,
            });

            _client.SendMessage(_messages.Last().TextoMensagem);
            this.tbInput.Clear();      
        }

        //inicia a conexão
        private void btnStart_Click(object sender, EventArgs e)
        {
            IPAddress ip;
            int port;

            if (IPAddress.TryParse(this.tbServerIP.Text, out ip) && Int32.TryParse(this.nudPort.Value.ToString(), out port) && ValidadeAllTextBoxes())
            {                
                _client.Connect(port, ip, tbUsername.Text, tbPassword.Text, tbNick.Text);
                handleGUIElements();
            }
            else
            {
                MessageBox.Show("Invalid TCP Port/IP Address or some field is empty.");
            }
        }


        //verifica se todos textboxes estão preenchidos
        private bool ValidadeAllTextBoxes()
        {
            foreach(Control c in gbInfo.Controls)
            {
                if(String.IsNullOrEmpty(c.Text))
                {
                    return false;
                }          
            }
            return true;
        }

        //inverte os estados dos componentes de entrada
        private void handleGUIElements()
        {
            foreach(Control c in gbInfo.Controls)
            {
                c.Enabled ^= true;
            }
            btnSend.Enabled ^= true;
            tbInput.Enabled ^= true;
        }


        //adiciona timestamp na mensagem
        private string FormatLogMessage(string message)
        {
            return String.Format("{0} : {1}", System.DateTime.Now.ToShortTimeString(), message);
        }
    }
}
