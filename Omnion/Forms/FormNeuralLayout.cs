using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omnion.Forms
{
    public partial class FormNeuralLayout : Form
    {
        private FormMain myParent = null;

        public FormNeuralLayout()
        {
            InitializeComponent();
        }

        public FormMain MyParent { get => myParent; set => myParent = value; }
    }
}
