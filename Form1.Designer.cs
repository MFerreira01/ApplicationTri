namespace ApplicationTri
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbImageCam = new System.Windows.Forms.PictureBox();
            this.BoutonACQ = new System.Windows.Forms.Button();
            this.labelAdressIP = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pbImageCapture = new System.Windows.Forms.PictureBox();
            this.labelDécision = new System.Windows.Forms.Label();
            this.tbCom = new System.Windows.Forms.TextBox();
            this.lblConnection = new System.Windows.Forms.Label();
            this.ouvrirImage = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.interfaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acquisitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controleRobotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.boutOuvrir = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.boutHisto = new System.Windows.Forms.Button();
            this.serialPortArduino = new System.IO.Ports.SerialPort(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCapture)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImageCam
            // 
            this.pbImageCam.Enabled = false;
            this.pbImageCam.Location = new System.Drawing.Point(9, 54);
            this.pbImageCam.Margin = new System.Windows.Forms.Padding(2);
            this.pbImageCam.Name = "pbImageCam";
            this.pbImageCam.Size = new System.Drawing.Size(525, 480);
            this.pbImageCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImageCam.TabIndex = 0;
            this.pbImageCam.TabStop = false;
            // 
            // BoutonACQ
            // 
            this.BoutonACQ.Location = new System.Drawing.Point(656, 405);
            this.BoutonACQ.Margin = new System.Windows.Forms.Padding(2);
            this.BoutonACQ.Name = "BoutonACQ";
            this.BoutonACQ.Size = new System.Drawing.Size(100, 53);
            this.BoutonACQ.TabIndex = 1;
            this.BoutonACQ.Text = "ACQ";
            this.BoutonACQ.UseVisualStyleBackColor = true;
            this.BoutonACQ.Click += new System.EventHandler(this.BoutonACQ_Click);
            // 
            // labelAdressIP
            // 
            this.labelAdressIP.AutoSize = true;
            this.labelAdressIP.Location = new System.Drawing.Point(619, 141);
            this.labelAdressIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAdressIP.Name = "labelAdressIP";
            this.labelAdressIP.Size = new System.Drawing.Size(51, 20);
            this.labelAdressIP.TabIndex = 2;
            this.labelAdressIP.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.OnTimer);
            // 
            // pbImageCapture
            // 
            this.pbImageCapture.Location = new System.Drawing.Point(9, 558);
            this.pbImageCapture.Margin = new System.Windows.Forms.Padding(2);
            this.pbImageCapture.Name = "pbImageCapture";
            this.pbImageCapture.Size = new System.Drawing.Size(525, 480);
            this.pbImageCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImageCapture.TabIndex = 4;
            this.pbImageCapture.TabStop = false;
            // 
            // labelDécision
            // 
            this.labelDécision.AutoSize = true;
            this.labelDécision.Location = new System.Drawing.Point(652, 558);
            this.labelDécision.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDécision.Name = "labelDécision";
            this.labelDécision.Size = new System.Drawing.Size(70, 20);
            this.labelDécision.TabIndex = 5;
            this.labelDécision.Text = "Décision";
            // 
            // tbCom
            // 
            this.tbCom.Location = new System.Drawing.Point(1040, 10);
            this.tbCom.Multiline = true;
            this.tbCom.Name = "tbCom";
            this.tbCom.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCom.Size = new System.Drawing.Size(391, 413);
            this.tbCom.TabIndex = 8;
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.BackColor = System.Drawing.Color.White;
            this.lblConnection.Location = new System.Drawing.Point(619, 91);
            this.lblConnection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(167, 20);
            this.lblConnection.TabIndex = 11;
            this.lblConnection.Text = "Connection en cours...";
            // 
            // ouvrirImage
            // 
            this.ouvrirImage.FileName = "openFileDialog1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interfaceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1443, 33);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // interfaceToolStripMenuItem
            // 
            this.interfaceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acquisitionToolStripMenuItem,
            this.controleRobotToolStripMenuItem});
            this.interfaceToolStripMenuItem.Name = "interfaceToolStripMenuItem";
            this.interfaceToolStripMenuItem.Size = new System.Drawing.Size(96, 29);
            this.interfaceToolStripMenuItem.Text = "Interface";
            // 
            // acquisitionToolStripMenuItem
            // 
            this.acquisitionToolStripMenuItem.Name = "acquisitionToolStripMenuItem";
            this.acquisitionToolStripMenuItem.Size = new System.Drawing.Size(232, 34);
            this.acquisitionToolStripMenuItem.Text = "Acquisition";
            // 
            // controleRobotToolStripMenuItem
            // 
            this.controleRobotToolStripMenuItem.Name = "controleRobotToolStripMenuItem";
            this.controleRobotToolStripMenuItem.Size = new System.Drawing.Size(232, 34);
            this.controleRobotToolStripMenuItem.Text = "Controle robot";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(613, 192);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(303, 181);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // boutOuvrir
            // 
            this.boutOuvrir.Location = new System.Drawing.Point(793, 405);
            this.boutOuvrir.Name = "boutOuvrir";
            this.boutOuvrir.Size = new System.Drawing.Size(104, 53);
            this.boutOuvrir.TabIndex = 14;
            this.boutOuvrir.Text = "Ouvrir";
            this.boutOuvrir.UseVisualStyleBackColor = true;
            this.boutOuvrir.Click += new System.EventHandler(this.boutOuvrir_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // boutHisto
            // 
            this.boutHisto.Location = new System.Drawing.Point(782, 558);
            this.boutHisto.Name = "boutHisto";
            this.boutHisto.Size = new System.Drawing.Size(104, 38);
            this.boutHisto.TabIndex = 15;
            this.boutHisto.Text = "histo";
            this.boutHisto.UseVisualStyleBackColor = true;
            this.boutHisto.Click += new System.EventHandler(this.boutHisto_Click);
            // 
            // serialPortArduino
            // 
            this.serialPortArduino.PortName = "COM5";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1443, 931);
            this.Controls.Add(this.boutHisto);
            this.Controls.Add(this.boutOuvrir);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.tbCom);
            this.Controls.Add(this.labelDécision);
            this.Controls.Add(this.pbImageCapture);
            this.Controls.Add(this.labelAdressIP);
            this.Controls.Add(this.BoutonACQ);
            this.Controls.Add(this.pbImageCam);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "ApplicationTri";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCapture)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImageCam;
        private System.Windows.Forms.Button BoutonACQ;
        private System.Windows.Forms.Label labelAdressIP;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pbImageCapture;
        private System.Windows.Forms.Label labelDécision;
        private System.Windows.Forms.TextBox tbCom;
        private System.Windows.Forms.Label lblConnection;
        private System.Windows.Forms.OpenFileDialog ouvrirImage;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem interfaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acquisitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controleRobotToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button boutOuvrir;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button boutHisto;
        private System.IO.Ports.SerialPort serialPortArduino;
    }
}

