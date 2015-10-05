using MessagingServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TesteComunicaçãoSocket
{
    public partial class Main : Form
    {
        private BindingList<Mensagem> messages;
        private TextWriter writer;

        public Main()
        {
            InitializeComponent();
            this.messages = new BindingList<Mensagem>();
            this.writer = new TextBoxStreamWriter(this.tbOutput);
            Console.SetOut(this.writer);
        }

        //refatorar
        private void button1_Click(object sender, EventArgs e)
        {
            this.messages.Add(new Mensagem {
                TextoMensagem = this.tbInput.Text,     
                TimeStamp = System.DateTime.Now,
            });

            Console.WriteLine(String.Format("{0} : {1}", messages.Last().TimeStamp.ToString(), messages.Last().TextoMensagem));
            this.tbInput.Clear();
        }

        //refatorar
        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.messages.Add(new Mensagem
                {
                    TextoMensagem = this.tbInput.Text,
                    TimeStamp = System.DateTime.Now,
                });

                Console.WriteLine(String.Format("{0} : {1}", messages.Last().TimeStamp.ToString(), messages.Last().TextoMensagem));
                this.tbInput.Clear();
            }
        }


    }
}
