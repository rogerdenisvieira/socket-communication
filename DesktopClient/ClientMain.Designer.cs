namespace TesteComunicaçãoSocket
{
    partial class ClientMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbInput = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.lblNick = new System.Windows.Forms.Label();
            this.tbNick = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbServerIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.btnStart = new System.Windows.Forms.Button();
            this.gbInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tbInput
            // 
            this.tbInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInput.Enabled = false;
            this.tbInput.Location = new System.Drawing.Point(12, 295);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(441, 20);
            this.tbInput.TabIndex = 0;
            this.tbInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbInput_KeyDown);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(459, 295);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 20);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.Location = new System.Drawing.Point(12, 97);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(522, 189);
            this.tbOutput.TabIndex = 2;
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.lblNick);
            this.gbInfo.Controls.Add(this.tbNick);
            this.gbInfo.Controls.Add(this.tbPassword);
            this.gbInfo.Controls.Add(this.lblPassword);
            this.gbInfo.Controls.Add(this.lblUsername);
            this.gbInfo.Controls.Add(this.tbUsername);
            this.gbInfo.Controls.Add(this.tbServerIP);
            this.gbInfo.Controls.Add(this.lblServerIP);
            this.gbInfo.Controls.Add(this.lblPort);
            this.gbInfo.Controls.Add(this.nudPort);
            this.gbInfo.Controls.Add(this.btnStart);
            this.gbInfo.Location = new System.Drawing.Point(12, 12);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(522, 79);
            this.gbInfo.TabIndex = 3;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "Connection Data";
            // 
            // lblNick
            // 
            this.lblNick.AutoSize = true;
            this.lblNick.Location = new System.Drawing.Point(369, 21);
            this.lblNick.Name = "lblNick";
            this.lblNick.Size = new System.Drawing.Size(32, 13);
            this.lblNick.TabIndex = 33;
            this.lblNick.Text = "Nick:";
            // 
            // tbNick
            // 
            this.tbNick.Location = new System.Drawing.Point(407, 18);
            this.tbNick.Name = "tbNick";
            this.tbNick.Size = new System.Drawing.Size(109, 20);
            this.tbNick.TabIndex = 32;
            this.tbNick.Text = "R0G3R";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(251, 18);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(109, 20);
            this.tbPassword.TabIndex = 31;
            this.tbPassword.Text = "mandolate";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(186, 21);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 30;
            this.lblPassword.Text = "Password:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(6, 21);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(58, 13);
            this.lblUsername.TabIndex = 29;
            this.lblUsername.Text = "Username:";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(71, 18);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(109, 20);
            this.tbUsername.TabIndex = 28;
            this.tbUsername.Text = "rogervieira";
            // 
            // tbServerIP
            // 
            this.tbServerIP.Location = new System.Drawing.Point(251, 44);
            this.tbServerIP.Name = "tbServerIP";
            this.tbServerIP.Size = new System.Drawing.Size(109, 20);
            this.tbServerIP.TabIndex = 27;
            this.tbServerIP.Text = "127.0.0.1";
            this.tbServerIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(186, 48);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(54, 13);
            this.lblServerIP.TabIndex = 26;
            this.lblServerIP.Text = "Server IP:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(6, 48);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(53, 13);
            this.lblPort.TabIndex = 25;
            this.lblPort.Text = "TCP Port:";
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(71, 44);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Minimum = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(109, 20);
            this.nudPort.TabIndex = 24;
            this.nudPort.Value = new decimal(new int[] {
            1025,
            0,
            0,
            0});
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(407, 42);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(109, 23);
            this.btnStart.TabIndex = 23;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // ClientMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 327);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbInput);
            this.MaximizeBox = false;
            this.Name = "ClientMain";
            this.Text = "Client";
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.GroupBox gbInfo;
        private System.Windows.Forms.Label lblNick;
        private System.Windows.Forms.TextBox tbNick;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbServerIP;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.Button btnStart;
    }
}

