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
            string _good = "";
            // double _price = Convert.ToDouble(price);
            // double _amount = Convert.ToDouble(amount);
            // double _sum = _price * _amount;
            double _price = 0;
            double _amount = 0;
            double _sum = 0;
            string _sum_str = _sum.ToString();
            DataGridViewRowCollection rows = dataTable.Rows;
            String date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            _type = rbtn_purchase.Checked ? "進貨" : "出貨";

            // for test
            richTextBox1.Text = $"{_sum_str} {_type} {_good}";

            // catch exceoption
            if (cbox_good.SelectedItem == null)
            {
                MessageBox.Show("必須選擇貨品");
                return;
            } else
            {
                _good = cbox_good.SelectedItem.ToString();
            }

            if (rbtn_purchase.Checked == false && rbtn_sale.Checked == false)
            {
                MessageBox.Show("請選擇進出貨");
                return;
            }

            if (_good.Trim() == "" || _price.ToString().Trim() == "" || _amount.ToString().Trim() == "")
            {
                MessageBox.Show("輸入欄位不得為空");
                return;
            }

            try {
                _price = Convert.ToDouble(price);
                _amount = Convert.ToDouble(amount);
                _sum = _price * _amount;
            } catch (Exception ex)
            {
                MessageBox.Show("單價與數量必須是數字");
                return;
            }

            // add data to dataTable
            rows.Add(new object[] {1, date, _type, _good, price, amount, _sum});
            MessageBox.Show(rows[0].Cells[0].Value.ToString());

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About is work");
        }
    }
}
