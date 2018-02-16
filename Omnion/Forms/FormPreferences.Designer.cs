namespace Omnion.Forms
{
    partial class FormPreferences
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
            this.folderBrowserDialogSaveDir = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSaveDir = new System.Windows.Forms.Button();
            this.gradientButtonResetToDefaults = new Omnion.CustomControls.GradientButton();
            this.textBoxSaveDir = new System.Windows.Forms.TextBox();
            this.checkBoxCompressRecordings = new System.Windows.Forms.CheckBox();
            this.gradientButtonSave = new Omnion.CustomControls.GradientButton();
            this.label3 = new System.Windows.Forms.Label();
            this.gradientButtonCancel = new Omnion.CustomControls.GradientButton();
            this.gradientPanel21 = new Omnion.CustomControls.GradientPanel2();
            this.panel1.SuspendLayout();
            this.gradientPanel21.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.checkBoxCompressRecordings);
            this.panel1.Controls.Add(this.textBoxSaveDir);
            this.panel1.Controls.Add(this.gradientButtonResetToDefaults);
            this.panel1.Controls.Add(this.buttonSaveDir);
            this.panel1.Location = new System.Drawing.Point(11, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(691, 235);
            this.panel1.TabIndex = 84;
            // 
            // buttonSaveDir
            // 
            this.buttonSaveDir.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSaveDir.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSaveDir.Location = new System.Drawing.Point(566, 81);
            this.buttonSaveDir.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSaveDir.Name = "buttonSaveDir";
            this.buttonSaveDir.Size = new System.Drawing.Size(37, 20);
            this.buttonSaveDir.TabIndex = 0;
            this.buttonSaveDir.Text = "...";
            this.buttonSaveDir.UseVisualStyleBackColor = true;
            this.buttonSaveDir.Click += new System.EventHandler(this.buttonSaveDir_Click);
            // 
            // gradientButtonResetToDefaults
            // 
            this.gradientButtonResetToDefaults.Active = false;
            this.gradientButtonResetToDefaults.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gradientButtonResetToDefaults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButtonResetToDefaults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButtonResetToDefaults.Location = new System.Drawing.Point(557, 16);
            this.gradientButtonResetToDefaults.Name = "gradientButtonResetToDefaults";
            this.gradientButtonResetToDefaults.Size = new System.Drawing.Size(113, 20);
            this.gradientButtonResetToDefaults.TabIndex = 87;
            this.gradientButtonResetToDefaults.Text = "reset to defaults";
            this.gradientButtonResetToDefaults.UseVisualStyleBackColor = false;
            this.gradientButtonResetToDefaults.Click += new System.EventHandler(this.gradientButtonResetToDefaults_Click);
            // 
            // textBoxSaveDir
            // 
            this.textBoxSaveDir.Location = new System.Drawing.Point(199, 81);
            this.textBoxSaveDir.Name = "textBoxSaveDir";
            this.textBoxSaveDir.Size = new System.Drawing.Size(368, 20);
            this.textBoxSaveDir.TabIndex = 55;
            // 
            // checkBoxCompressRecordings
            // 
            this.checkBoxCompressRecordings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxCompressRecordings.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxCompressRecordings.Checked = true;
            this.checkBoxCompressRecordings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCompressRecordings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCompressRecordings.Location = new System.Drawing.Point(57, 118);
            this.checkBoxCompressRecordings.Name = "checkBoxCompressRecordings";
            this.checkBoxCompressRecordings.Padding = new System.Windows.Forms.Padding(0, 0, 4, 3);
            this.checkBoxCompressRecordings.Size = new System.Drawing.Size(151, 20);
            this.checkBoxCompressRecordings.TabIndex = 132;
            this.checkBoxCompressRecordings.Text = "Compress Recordings:     ";
            this.checkBoxCompressRecordings.UseVisualStyleBackColor = false;
            // 
            // gradientButtonSave
            // 
            this.gradientButtonSave.Active = false;
            this.gradientButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gradientButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButtonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButtonSave.Location = new System.Drawing.Point(119, 275);
            this.gradientButtonSave.Name = "gradientButtonSave";
            this.gradientButtonSave.Size = new System.Drawing.Size(127, 32);
            this.gradientButtonSave.TabIndex = 49;
            this.gradientButtonSave.Text = "Save";
            this.gradientButtonSave.UseVisualStyleBackColor = false;
            this.gradientButtonSave.Click += new System.EventHandler(this.gradientButtonSave_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(69, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 54;
            this.label3.Text = "Directory saved recordings:";
            // 
            // gradientButtonCancel
            // 
            this.gradientButtonCancel.Active = false;
            this.gradientButtonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gradientButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gradientButtonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gradientButtonCancel.Location = new System.Drawing.Point(447, 273);
            this.gradientButtonCancel.Name = "gradientButtonCancel";
            this.gradientButtonCancel.Size = new System.Drawing.Size(127, 32);
            this.gradientButtonCancel.TabIndex = 50;
            this.gradientButtonCancel.Text = "Cancel";
            this.gradientButtonCancel.UseVisualStyleBackColor = false;
            this.gradientButtonCancel.Click += new System.EventHandler(this.gradientButtonCancel_Click);
            // 
            // gradientPanel21
            // 
            this.gradientPanel21.Controls.Add(this.gradientButtonCancel);
            this.gradientPanel21.Controls.Add(this.label3);
            this.gradientPanel21.Controls.Add(this.gradientButtonSave);
            this.gradientPanel21.Controls.Add(this.panel1);
            this.gradientPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientPanel21.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel21.Name = "gradientPanel21";
            this.gradientPanel21.Size = new System.Drawing.Size(717, 320);
            this.gradientPanel21.TabIndex = 51;
            // 
            // FormPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 320);
            this.Controls.Add(this.gradientPanel21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPreferences";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gradientPanel21.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogSaveDir;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.CheckBox checkBoxCompressRecordings;
        internal System.Windows.Forms.TextBox textBoxSaveDir;
        internal CustomControls.GradientButton gradientButtonResetToDefaults;
        private System.Windows.Forms.Button buttonSaveDir;
        internal CustomControls.GradientButton gradientButtonSave;
        private System.Windows.Forms.Label label3;
        private CustomControls.GradientButton gradientButtonCancel;
        private CustomControls.GradientPanel2 gradientPanel21;
    }
}