using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Omnion.Forms
{
    public partial class FormPreferences : Form
    {
        private FormMain myParent = null;

        public FormPreferences()
        {
            InitializeComponent();
        }

        public FormMain MyParent
        {
            get
            {
                return myParent;
            }

            set
            {
                myParent = value;
            }
        }

        private void gradientButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void Initialize()
        {
            textBoxSaveDir.Text = myParent.SaveDir;
            checkBoxCompressRecordings.Checked = myParent.CompressRecordings;
        }

        private void gradientButtonSave_Click(object sender, EventArgs e)
        {
            myParent.SaveDir = textBoxSaveDir.Text;
            myParent.CompressRecordings = checkBoxCompressRecordings.Checked;

            Close();
        }

        private void buttonSaveDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogSaveDir.ShowDialog() == DialogResult.OK)
            {
                textBoxSaveDir.Text = folderBrowserDialogSaveDir.SelectedPath;
            }
        }

        private void gradientButtonResetToDefaults_Click(object sender, EventArgs e)
        {
            myParent.ResetToDefaults();
            Initialize();
        }

        private void gradientButtonNow_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
        }

    }
}
