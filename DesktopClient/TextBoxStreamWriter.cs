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
            output.AppendText(value.ToString());
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }


}
