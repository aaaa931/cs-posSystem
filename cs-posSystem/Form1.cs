using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_posSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_input_Click(object sender, EventArgs e)
        {
            string price = tbox_price.Text;
            string amount = tbox_amount.Text;
            string _type = "";
            string _good = cbox_good.SelectedItem.ToString();
            double _price = Convert.ToDouble(price);
            double _amount = Convert.ToDouble(amount);
            double _sum = _price * _amount;
            string _sum_str = _sum.ToString();

            if (rbtn_purchase.Checked)
            {
                _type = "進貨";
            } else
            {
                _type = "出貨";
            }

            richTextBox1.Text = $"{_sum_str} {_type} {_good}";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About is work");
        }
    }
}
