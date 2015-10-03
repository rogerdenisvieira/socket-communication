using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TesteComunicaçãoSocket
{
    public partial class Main : Form
    {
        private BindingList<Mensagem> messages;

        public Main()
        {
            InitializeComponent();
            this.messages = new BindingList<Mensagem>();            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.messages.Add(new Mensagem {
                TextoMensagem = this.textBox1.Text,     
                TimeStamp = System.DateTime.Now,
            });

           this.textBox2.Text += String.Format("{0} : {1}{2}",messages.Last().TimeStamp.ToString(), messages.Last().TextoMensagem,Environment.NewLine);
           //MessageBox.Show(messages.Last().TextoMensagem);
            //this.dataGridView1.Update();
        }


    }
}
