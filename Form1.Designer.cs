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
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonInit = new System.Windows.Forms.Button();
            this.pbImageCapture = new System.Windows.Forms.PictureBox();
            this.labelDécision = new System.Windows.Forms.Label();
            this.boutServeur = new System.Windows.Forms.Button();
            this.boutClient = new System.Windows.Forms.Button();
            this.tbCom = new System.Windows.Forms.TextBox();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.boutQuit = new System.Windows.Forms.Button();
            this.lblConnection = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCapture)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImageCam
            // 
            this.pbImageCam.Enabled = false;
            this.pbImageCam.Location = new System.Drawing.Point(22, 10);
            this.pbImageCam.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbImageCam.Name = "pbImageCam";
            this.pbImageCam.Size = new System.Drawing.Size(525, 480);
            this.pbImageCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImageCam.TabIndex = 0;
            this.pbImageCam.TabStop = false;
            // 
            // BoutonACQ
            // 
            this.BoutonACQ.Location = new System.Drawing.Point(656, 155);
            this.BoutonACQ.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BoutonACQ.Name = "BoutonACQ";
            this.BoutonACQ.Size = new System.Drawing.Size(100, 53);
            this.BoutonACQ.TabIndex = 1;
            this.BoutonACQ.Text = "ACQ";
            this.BoutonACQ.UseVisualStyleBackColor = true;
            this.BoutonACQ.Click += new System.EventHandler(this.BoutonACQ_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(577, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.OnTimer);
            // 
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(656, 82);
            this.buttonInit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(102, 53);
            this.buttonInit.TabIndex = 3;
            this.buttonInit.Text = "init";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // pbImageCapture
            // 
            this.pbImageCapture.Location = new System.Drawing.Point(22, 515);
            this.pbImageCapture.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbImageCapture.Name = "pbImageCapture";
            this.pbImageCapture.Size = new System.Drawing.Size(525, 282);
            this.pbImageCapture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImageCapture.TabIndex = 4;
            this.pbImageCapture.TabStop = false;
            // 
            // labelDécision
            // 
            this.labelDécision.AutoSize = true;
            this.labelDécision.Location = new System.Drawing.Point(652, 268);
            this.labelDécision.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDécision.Name = "labelDécision";
            this.labelDécision.Size = new System.Drawing.Size(70, 20);
            this.labelDécision.TabIndex = 5;
            this.labelDécision.Text = "Décision";
            // 
            // boutServeur
            // 
            this.boutServeur.Location = new System.Drawing.Point(869, 82);
            this.boutServeur.Name = "boutServeur";
            this.boutServeur.Size = new System.Drawing.Size(107, 53);
            this.boutServeur.TabIndex = 6;
            this.boutServeur.Text = "Serveur";
            this.boutServeur.UseVisualStyleBackColor = true;
            this.boutServeur.Click += new System.EventHandler(this.boutServeur_Click);
            // 
            // boutClient
            // 
            this.boutClient.Location = new System.Drawing.Point(869, 155);
            this.boutClient.Name = "boutClient";
            this.boutClient.Size = new System.Drawing.Size(107, 53);
            this.boutClient.TabIndex = 7;
            this.boutClient.Text = "Client";
            this.boutClient.UseVisualStyleBackColor = true;
            this.boutClient.Click += new System.EventHandler(this.boutClient_Click);
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
            // tbMessage
            // 
            this.tbMessage.Location = new System.Drawing.Point(1040, 440);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(391, 26);
            this.tbMessage.TabIndex = 9;
            this.tbMessage.Text = "Ceci est un message test";
            // 
            // boutQuit
            // 
            this.boutQuit.Location = new System.Drawing.Point(869, 394);
            this.boutQuit.Name = "boutQuit";
            this.boutQuit.Size = new System.Drawing.Size(84, 29);
            this.boutQuit.TabIndex = 10;
            this.boutQuit.Text = "Quitter";
            this.boutQuit.UseVisualStyleBackColor = true;
            this.boutQuit.Click += new System.EventHandler(this.boutQuit_Click);
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.BackColor = System.Drawing.Color.White;
            this.lblConnection.Location = new System.Drawing.Point(577, 10);
            this.lblConnection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(167, 20);
            this.lblConnection.TabIndex = 11;
            this.lblConnection.Text = "Connection en cours...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1443, 807);
            this.Controls.Add(this.lblConnection);
            this.Controls.Add(this.boutQuit);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.tbCom);
            this.Controls.Add(this.boutClient);
            this.Controls.Add(this.boutServeur);
            this.Controls.Add(this.labelDécision);
            this.Controls.Add(this.pbImageCapture);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BoutonACQ);
            this.Controls.Add(this.pbImageCam);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "ApplicationTri";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCapture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImageCam;
        private System.Windows.Forms.Button BoutonACQ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonInit;
        private System.Windows.Forms.PictureBox pbImageCapture;
        private System.Windows.Forms.Label labelDécision;
        private System.Windows.Forms.Button boutServeur;
        private System.Windows.Forms.Button boutClient;
        private System.Windows.Forms.TextBox tbCom;
        private System.Windows.Forms.TextBox tbMessage;
        private System.Windows.Forms.Button boutQuit;
        private System.Windows.Forms.Label lblConnection;
    }
}

