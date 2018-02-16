namespace Omnion.Forms
{
    partial class FormLoadSave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoadSave));
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxNumberOfObjects = new System.Windows.Forms.TextBox();
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.gradientButtonCancel = new Omnion.CustomControls.GradientButton();
            this.gradientButtonLoadSave = new Omnion.CustomControls.GradientButton();
            this.gradientPanel21 = new Omnion.CustomControls.GradientPanel2();
            this.panelBusy = new System.Windows.Forms.Panel();
            this.progressBarLoadSave = new System.Windows.Forms.ProgressBar();
            this.labelBusy = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxCalcsDone = new System.Windows.Forms.TextBox();
            this.listBoxRecordings = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gradientButtonDelete = new Omnion.CustomControls.GradientButton();
            this.gradientPanel21.SuspendLayout();
            this.panelBusy.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(57, 28);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(184, 20);
            this.textBoxName.TabIndex = 1;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // textBoxNumberOfObjects
            // 
            this.textBoxNumberOfObjects.Location = new System.Drawing.Point(499, 28);
            this.textBoxNumberOfObjects.Name = "textBoxNumberOfObjects";
            this.textBoxNumberOfObjects.ReadOnly = true;
            this.textBoxNumberOfObjects.Size = new System.Drawing.Size(52, 20);
            this.textBoxNumberOfObjects.TabIndex = 3;
            // 
            // textBoxDate
            // 
            this.textBoxDate.Location = new System.Drawing.Point(281, 28);
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.ReadOnly = true;
            this.textBoxDate.Size = new System.Drawing.Size(110, 20);
            this.textBoxDate.TabIndex = 5;
            // 
            // gradientButtonCancel
            // 
            this.gradientButtonCancel.Active = false;
            this.gradientButtonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gradientButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButtonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButtonCancel.Location = new System.Drawing.Point(541, 382);
            this.gradientButtonCancel.Name = "gradientButtonCancel";
            this.gradientButtonCancel.Size = new System.Drawing.Size(127, 32);
            this.gradientButtonCancel.TabIndex = 48;
            this.gradientButtonCancel.Text = "Cancel";
            this.gradientButtonCancel.UseVisualStyleBackColor = false;
            this.gradientButtonCancel.Click += new System.EventHandler(this.gradientButtonCancel_Click);
            // 
            // gradientButtonLoadSave
            // 
            this.gradientButtonLoadSave.Active = false;
            this.gradientButtonLoadSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gradientButtonLoadSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButtonLoadSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButtonLoadSave.Location = new System.Drawing.Point(135, 381);
            this.gradientButtonLoadSave.Name = "gradientButtonLoadSave";
            this.gradientButtonLoadSave.Size = new System.Drawing.Size(127, 32);
            this.gradientButtonLoadSave.TabIndex = 43;
            this.gradientButtonLoadSave.Text = "Save";
            this.gradientButtonLoadSave.UseVisualStyleBackColor = false;
            this.gradientButtonLoadSave.Click += new System.EventHandler(this.gradientButtonLoadSave_Click);
            // 
            // gradientPanel21
            // 
            this.gradientPanel21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gradientPanel21.Controls.Add(this.panelBusy);
            this.gradientPanel21.Controls.Add(this.label1);
            this.gradientPanel21.Controls.Add(this.label3);
            this.gradientPanel21.Controls.Add(this.label2);
            this.gradientPanel21.Controls.Add(this.label6);
            this.gradientPanel21.Controls.Add(this.textBoxCalcsDone);
            this.gradientPanel21.Controls.Add(this.listBoxRecordings);
            this.gradientPanel21.Controls.Add(this.label4);
            this.gradientPanel21.Controls.Add(this.panel1);
            this.gradientPanel21.Controls.Add(this.panel2);
            this.gradientPanel21.Location = new System.Drawing.Point(0, 1);
            this.gradientPanel21.Name = "gradientPanel21";
            this.gradientPanel21.Size = new System.Drawing.Size(873, 435);
            this.gradientPanel21.TabIndex = 47;
            // 
            // panelBusy
            // 
            this.panelBusy.BackColor = System.Drawing.Color.Silver;
            this.panelBusy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBusy.Controls.Add(this.progressBarLoadSave);
            this.panelBusy.Controls.Add(this.labelBusy);
            this.panelBusy.Location = new System.Drawing.Point(204, 140);
            this.panelBusy.Name = "panelBusy";
            this.panelBusy.Size = new System.Drawing.Size(415, 135);
            this.panelBusy.TabIndex = 50;
            this.panelBusy.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBusy_Paint);
            // 
            // progressBarLoadSave
            // 
            this.progressBarLoadSave.Location = new System.Drawing.Point(81, 82);
            this.progressBarLoadSave.Name = "progressBarLoadSave";
            this.progressBarLoadSave.Size = new System.Drawing.Size(227, 27);
            this.progressBarLoadSave.TabIndex = 1;
            // 
            // labelBusy
            // 
            this.labelBusy.AutoSize = true;
            this.labelBusy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelBusy.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBusy.Location = new System.Drawing.Point(72, 20);
            this.labelBusy.Name = "labelBusy";
            this.labelBusy.Size = new System.Drawing.Size(186, 51);
            this.labelBusy.TabIndex = 0;
            this.labelBusy.Text = "loading..";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 20);
            this.label1.TabIndex = 48;
            this.label1.Text = "Existing Recordings:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "Date:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(402, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Neurons(Layers):";
            // 
            // textBoxCalcsDone
            // 
            this.textBoxCalcsDone.Location = new System.Drawing.Point(650, 28);
            this.textBoxCalcsDone.Name = "textBoxCalcsDone";
            this.textBoxCalcsDone.ReadOnly = true;
            this.textBoxCalcsDone.Size = new System.Drawing.Size(100, 20);
            this.textBoxCalcsDone.TabIndex = 7;
            // 
            // listBoxRecordings
            // 
            this.listBoxRecordings.DisplayMember = "DisplayText";
            this.listBoxRecordings.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxRecordings.FormattingEnabled = true;
            this.listBoxRecordings.ItemHeight = 16;
            this.listBoxRecordings.Location = new System.Drawing.Point(22, 112);
            this.listBoxRecordings.Name = "listBoxRecordings";
            this.listBoxRecordings.Size = new System.Drawing.Size(824, 228);
            this.listBoxRecordings.TabIndex = 45;
            this.listBoxRecordings.ValueMember = "FileName";
            this.listBoxRecordings.SelectedIndexChanged += new System.EventHandler(this.listBoxRecordings_SelectedIndexChanged);
            this.listBoxRecordings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxRecordings_MouseDoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(558, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Training iterations:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(13, 82);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(844, 277);
            this.panel1.TabIndex = 49;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.gradientButtonDelete);
            this.panel2.Location = new System.Drawing.Point(13, 12);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(844, 48);
            this.panel2.TabIndex = 0;
            // 
            // gradientButtonDelete
            // 
            this.gradientButtonDelete.Active = false;
            this.gradientButtonDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gradientButtonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButtonDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButtonDelete.Location = new System.Drawing.Point(753, 14);
            this.gradientButtonDelete.Name = "gradientButtonDelete";
            this.gradientButtonDelete.Size = new System.Drawing.Size(74, 20);
            this.gradientButtonDelete.TabIndex = 50;
            this.gradientButtonDelete.Text = "Delete";
            this.gradientButtonDelete.UseVisualStyleBackColor = false;
            this.gradientButtonDelete.Click += new System.EventHandler(this.gradientButtonDelete_Click);
            // 
            // FormLoadSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 435);
            this.Controls.Add(this.gradientButtonCancel);
            this.Controls.Add(this.gradientButtonLoadSave);
            this.Controls.Add(this.textBoxDate);
            this.Controls.Add(this.textBoxNumberOfObjects);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.gradientPanel21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLoadSave";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLoadSave";
            this.gradientPanel21.ResumeLayout(false);
            this.gradientPanel21.PerformLayout();
            this.panelBusy.ResumeLayout(false);
            this.panelBusy.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private CustomControls.GradientPanel2 gradientPanel21;
        private CustomControls.GradientButton gradientButtonCancel;
        internal CustomControls.GradientButton gradientButtonLoadSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox textBoxNumberOfObjects;
        internal System.Windows.Forms.TextBox textBoxDate;
        internal System.Windows.Forms.TextBox textBoxCalcsDone;
        internal System.Windows.Forms.ListBox listBoxRecordings;
        internal System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        internal CustomControls.GradientButton gradientButtonDelete;
        internal System.Windows.Forms.Panel panelBusy;
        internal System.Windows.Forms.Label labelBusy;
        internal System.Windows.Forms.ProgressBar progressBarLoadSave;
    }
}