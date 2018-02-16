namespace Omnion.Forms
{
    partial class FormNeuralLayout
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
            this.panel7 = new System.Windows.Forms.Panel();
            this.pictureBoxLayout = new System.Windows.Forms.PictureBox();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLayout)).BeginInit();
            this.SuspendLayout();
            // 
            // panel7
            // 
            this.panel7.AutoScroll = true;
            this.panel7.Controls.Add(this.pictureBoxLayout);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1297, 739);
            this.panel7.TabIndex = 9;
            // 
            // pictureBoxLayout
            // 
            this.pictureBoxLayout.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxLayout.Name = "pictureBoxLayout";
            this.pictureBoxLayout.Size = new System.Drawing.Size(731, 350);
            this.pictureBoxLayout.TabIndex = 8;
            this.pictureBoxLayout.TabStop = false;
            // 
            // FormNeuralLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 739);
            this.Controls.Add(this.panel7);
            this.Name = "FormNeuralLayout";
            this.Text = "FormNeuralLayout";
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLayout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel7;
        internal System.Windows.Forms.PictureBox pictureBoxLayout;
    }
}