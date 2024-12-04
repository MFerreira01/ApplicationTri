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
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCam)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImageCam
            // 
            this.pbImageCam.Location = new System.Drawing.Point(30, 12);
            this.pbImageCam.Name = "pbImageCam";
            this.pbImageCam.Size = new System.Drawing.Size(687, 581);
            this.pbImageCam.TabIndex = 0;
            this.pbImageCam.TabStop = false;
            // 
            // BoutonACQ
            // 
            this.BoutonACQ.Location = new System.Drawing.Point(875, 194);
            this.BoutonACQ.Name = "BoutonACQ";
            this.BoutonACQ.Size = new System.Drawing.Size(133, 66);
            this.BoutonACQ.TabIndex = 1;
            this.BoutonACQ.Text = "ACQ";
            this.BoutonACQ.UseVisualStyleBackColor = true;
            this.BoutonACQ.Click += new System.EventHandler(this.BoutonACQ_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(848, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 25);
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
            this.buttonInit.Location = new System.Drawing.Point(875, 103);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(136, 66);
            this.buttonInit.TabIndex = 3;
            this.buttonInit.Text = "init";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2054, 834);
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BoutonACQ);
            this.Controls.Add(this.pbImageCam);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pbImageCam)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImageCam;
        private System.Windows.Forms.Button BoutonACQ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonInit;
    }
}

