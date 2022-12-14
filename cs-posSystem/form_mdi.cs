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
    public partial class form_mdi : Form
    {
        public form_mdi()
        {
            InitializeComponent();
        }

        private void 開啟WordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // open pdf file
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "選擇 pdf 檔案";
            dlg.InitialDirectory = ".\\";
            dlg.Filter = "pdf files (*.pdf) | *.pdf";

            if (dlg.FileName != null && dlg.ShowDialog() == DialogResult.OK)
            {
                form_pdf form_pdf = new form_pdf(dlg.FileName);
                form_pdf.TopLevel = false;
                form_pdf.Parent = this;
                form_pdf.MdiParent = this;
                form_pdf.Show();
            }
        }

        private void 開啟WordToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 2.1. 找出 word 檔案位置
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "選擇 word 檔案";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "word files(*.*)| *.docx";
            if (dialog.FileName != null && dialog.ShowDialog() == DialogResult.OK)
            {
                form_word form_word = new form_word(dialog.FileName);
                form_word.MdiParent = this;
                form_word.Show();
            }
        }

        private void 開啟ExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1. 找出 excel 檔案位置
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "選擇 excel 檔案";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "excel files(*.*)| *.xlsx";
            if (dialog.FileName != null && dialog.ShowDialog() == DialogResult.OK)
            {
                form_excel form_excel = new form_excel(dialog.FileName);
                form_excel.MdiParent = this;
                form_excel.Show();
            }
        }

        private void 水平排版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void 垂直排版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
    }
}
