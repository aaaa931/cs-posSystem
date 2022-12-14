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
    public partial class Form2 : Form
    {
        bool isPwd = false;
        public Form2()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            String pwd = "1234";

            if (String.Equals(pwd, tbox_pwd.Text))
            {
                isPwd = true;
                this.Close();
            } else
            {
                MessageBox.Show("密碼錯誤");
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
            System.Environment.Exit(0);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.isPwd) e.Cancel = true;
        }
    }
}
