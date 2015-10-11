using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessagingServer
{
    class TextBoxStreamWriter : TextWriter
    {
        private TextBox output;

        public TextBoxStreamWriter(TextBox anOutput)
        {
            this.output = anOutput;
        }

        public override void Write(char value)
        {
            base.Write(value);            
            AppendTextBox(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

        public void AppendTextBox(string value)
        {
            if (output.InvokeRequired)
            {
                output.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            output.Text += value;
        }


    }


}
