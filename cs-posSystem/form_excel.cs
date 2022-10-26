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
using ExcelDataReader;

namespace cs_posSystem
{
    public partial class form_excel : Form
    {
        public form_excel()
        {
            InitializeComponent();
        }

        public form_excel(string dir)
        {
            InitializeComponent();
            // 2. 打開excel檔
            var stream = System.IO.File.Open(
                dir,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read
            );

            ExcelDataReader.IExcelDataReader reader =
            ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

            var conf = new ExcelDataSetConfiguration
            {
                ConfigureDataTable = _ => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = true
                }
            };

            DataSet result = reader.AsDataSet(conf);
            tableCollection = result.Tables;
            cbox_select.Items.Clear();
            foreach (DataTable table in tableCollection)
            {
                cbox_select.Items.Add(table.TableName);//add sheet to combobox}
            }
        }

        DataTableCollection tableCollection;
        private void cbox_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tableCollection[cbox_select.SelectedItem.ToString()];
            dataGridView1.DataSource = dt;
        }
    }
}
