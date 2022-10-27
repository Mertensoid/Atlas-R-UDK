using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Garant_R_NEW
{
    public partial class Form4 : Form
    {
        Form1 F1;
        public Form4(Form1 _F1)
        {
            F1 = _F1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            F1.sendCommand("actionWr " + textBox2.Text);
        }
    }
}
