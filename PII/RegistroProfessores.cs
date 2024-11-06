using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PII
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            registro reg = new registro();
            reg.ShowDialog();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 reg2 = new Form2();
            reg2.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form3 reg3 = new Form3();
            reg3.ShowDialog();
        }
    }
}
