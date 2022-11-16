using ExcelDataReader.Log;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using good;
// 動態綁定 Dynamic Binding
using System.Reflection;

namespace cs_posSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // show login form dialog
            Form2 from_login;
            from_login = new Form2();
            from_login.ShowDialog();
        }

        private void btn_input_Click(object sender, EventArgs e)
        {
            string price = tbox_price.Text;
            string amount = tbox_amount.Text;
            string _type = "";
            string _good = "";
            double _price = 0;
            double _amount = 0;
            DataGridViewRowCollection rows = dataTable.Rows;
            List<igood> poslist = new List<igood>();
            good.good newGood;

            _type = rbtn_purchase.Checked ? "進貨" : "出貨";

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
            } catch (Exception ex)
            {
                MessageBox.Show("單價與數量必須是數字");
                return;
            }

            if (_type == "進貨")
            {
                newGood = new good_input(_good, _price, _amount);
                newGood.showLog();
            } else
            {
                newGood = new good_output(_good, _price, _amount);
                newGood.showLog();
            }

            /*poslist.Add((pos.igood)Activator.CreateInstance(
                Type.GetType("pos.good_input_new"), new object[] { _good, _price, _amount }
                ));
            MessageBox.Show(poslist[0].name);*/

            // add data to dataTable
            rows.Add(new object[] {newGood._id, newGood._date, newGood._type, newGood._name, newGood._price, newGood._amount, newGood._total});
            // get datagridview value
            MessageBox.Show(rows[0].Cells[0].Value.ToString());
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("說明文件預定");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. 找出 excel 檔案位置
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "選擇 excel 檔案";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "excel files(*.*)| *.xlsx";
            if (dialog.FileName != null && dialog.ShowDialog() == DialogResult.OK)
            {
                form_excel form_excel = new form_excel(dialog.FileName);
                form_excel.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "pdf files (*.pdf) | *.pdf";

            if (dlg.FileName != null && dlg.ShowDialog() == DialogResult.OK)
            {
                form_pdf form_pdf = new form_pdf(dlg.FileName);
                form_pdf.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 2.1. 找出 word 檔案位置
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "選擇 word 檔案";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "word files(*.*)| *.docx";
            if (dialog.FileName != null && dialog.ShowDialog() == DialogResult.OK)
            {
                form_word form_word = new form_word(dialog.FileName);
                form_word.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            form_mdi form_Mdi = new form_mdi();
            form_Mdi.Show();
        }
    }
}

/*namespace pos
{
    interface igood
    {
        string name { get; }
        double price { get; }
        double amount { get; }
    }
    class good
    {
        private static int num_of_id = 0;
        public int _id { get; }
        public double _price { get; }
        public double _amount { get; }
        public double _total { get; }
        public tool.type _type { get; }
        public string _name { get; }
        public string _date { get; }

        public good(string name, tool.type type, double price, double amount)
        {
            num_of_id++;

            this._id = num_of_id;
            this._date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            this._name = name;
            this._price = price;
            this._amount = amount;
            this._type = type;
            this._total = _price * _amount;
        }

        public double getTotal() { 
            return _total;
        }

        public override string ToString()
        {
            return $"id: {_id}\ndate: {_date}\ntype: {_type}\nname: {_name}\nprice {_price}\namount: {_amount}\ntotal: {_total}";
        }

        public void showLog()
        {
            MessageBox.Show(this.ToString());
        }
        // rows.Add(new object[] {1, date, _type, _good, price, amount, _sum});
    }

    class good_input: good
    {
        public good_input(string name, double price, double amount) : base(name, tool.type.進貨, price, amount) { }
    }

    class good_output : good
    {
        public good_output(string name, double price, double amount) : base(name, tool.type.出貨, price, amount) { }
    }

    class tool
    {
        public enum type { 進貨, 出貨 }
    }
}*/