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
using good;
// 動態綁定 Dynamic Binding
using System.Reflection;
using System.Data.SQLite;
using Application = System.Windows.Forms.Application;
using Excel = Microsoft.Office.Interop.Excel;
using Classes;
using System.Diagnostics;

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

            Load_DB();
            Show_DB();
            this.label5.Text = index.ToString();
        }

        int index = 1;
        public class DBConfig
        {
            //log.db要放在【bin\Debug底下】      
            public static string dbFile = Application.StartupPath + @"\log.db";
            public static string dbPath = "Data source=" + dbFile;
            public static SQLiteConnection sqlite_connect;
            public static SQLiteCommand sqlite_cmd;
            public static SQLiteDataReader sqlite_datareader;
        }
        private void Load_DB()
        {
            DBConfig.sqlite_connect = new SQLiteConnection(DBConfig.dbPath);
            DBConfig.sqlite_connect.Open();// Open

        }
        private void Show_DB()
        {
            this.dataTable.Rows.Clear();

            string sql = @"SELECT * from record;";
            DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
            DBConfig.sqlite_datareader = DBConfig.sqlite_cmd.ExecuteReader();

            if (DBConfig.sqlite_datareader.HasRows)
            {
                while (DBConfig.sqlite_datareader.Read()) //read every data
                {
                    int _serial = Convert.ToInt32(DBConfig.sqlite_datareader["serial"]);
                    int _date = Convert.ToInt32(DBConfig.sqlite_datareader["date"]);
                    int _type = Convert.ToInt32(DBConfig.sqlite_datareader["type"]);
                    string _name = Convert.ToString(DBConfig.sqlite_datareader["name"]);
                    double _price = Convert.ToDouble(DBConfig.sqlite_datareader["price"]);
                    double _number = Convert.ToDouble(DBConfig.sqlite_datareader["number"]);
                    double _total = _price * _number;
                    string _date_str = DateTimeOffset.FromUnixTimeSeconds(_date).ToString("yyyy-MM-dd hh:mm:ss");
                    string _type_str = _type == 0 ? "進貨" : "出貨";

                    index = _serial;
                    DataGridViewRowCollection rows = dataTable.Rows;
                    rows.Add(new Object[] { index, _date_str, _type_str, _name, _price, _number
                                               , _total });
                }

                DBConfig.sqlite_datareader.Close();
            }
        }


        private void btn_input_Click(object sender, EventArgs e)
        {
            string _name = "";
            long _date = DateTimeOffset.Now.ToUnixTimeSeconds();
            int _type = 0;
            double _price = 0;
            double _number = 0;
            DataGridViewRowCollection rows = dataTable.Rows;
            good.good newGood;

            _type = rbtn_purchase.Checked ? 0 : 1;

            // catch exceoption
            if (cbox_good.SelectedItem == null)
            {
                MessageBox.Show("必須選擇貨品");
                return;
            } else
            {
                _name = cbox_good.SelectedItem.ToString();
            }

            if (rbtn_purchase.Checked == false && rbtn_sale.Checked == false)
            {
                MessageBox.Show("請選擇進出貨");
                return;
            }

            if (_name.Trim() == "" || _price.ToString().Trim() == "" || _number.ToString().Trim() == "")
            {
                MessageBox.Show("輸入欄位不得為空");
                return;
            }

            try {
                _price = Convert.ToDouble(tbox_price.Text);
                _number = Convert.ToDouble(tbox_amount.Text);
            } catch (Exception ex)
            {
                MessageBox.Show("單價與數量必須是數字");
                return;
            }

            this.index++;
            label5.Text = this.index.ToString();

            string sql = @"insert into record (date, type, name, price, number) values(" +
                " '" + _date.ToString() + "'," +
                " '" + _type.ToString() + "'," +
                " '" + _name.ToString() + "'," +
                " '" + _price.ToString() + "'," +
                " '" + _number.ToString() + "' " +
                ")";

            DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
            DBConfig.sqlite_cmd.ExecuteNonQuery();

            Show_DB();

            /*if (_type == 0)
            {
                newGood = new good_input(_name, _price, _number);
                newGood.showLog();
            } else
            {
                newGood = new good_output(_name, _price, _number);
                newGood.showLog();
            }*/

            // add data to dataTable
            // rows.Add(new object[] {newGood._id, newGood._date, newGood._type, newGood._name, newGood._price, newGood._amount, newGood._total});
            // get datagridview value
            // MessageBox.Show(rows[0].Cells[0].Value.ToString());
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("說明文件預定");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. 找出 excel 檔案位置
            //OpenFileDialog dialog = new OpenFileDialog();
            //dialog.Title = "選擇 excel 檔案";
            //dialog.InitialDirectory = ".\\";
            //dialog.Filter = "excel files(*.*)| *.xlsx";
            //if (dialog.FileName != null && dialog.ShowDialog() == DialogResult.OK)
            //{
            //    form_excel form_excel = new form_excel(dialog.FileName);
            //    form_excel.ShowDialog();
            //}

            //DataGridViewRowCollection rows = dataTable.Rows;
            //// 設定儲存excel檔
            //SaveFileDialog save = new SaveFileDialog();
            //save.InitialDirectory =
            //Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //save.FileName = "Export_Chart_Data";
            //save.Filter = "*.xlsx | *.xlsx";
            //if (save.ShowDialog() != DialogResult.OK) return;
            //// Excel 物件
            //Excel.Application xls = null;
            //try
            //{
            //    // 打開excel
            //    xls = new Excel.Application();
            //    // 新增第一個sheet
            //    // Excel WorkBook
            //    Excel.Workbook book = xls.Workbooks.Add();
            //    //Excel.Worksheet Sheet = (Excel.Worksheet)book.Worksheets[1];
            //    Excel.Worksheet Sheet = xls.ActiveSheet;
            //    // 把資料塞進 Excel 內
            //    // 標題
            //    string[] titles = new string[] { "編號", "日期", "類別", "名稱", "單價", "數量", "總價" };

            //    for (int i = 0; i < titles.Length; i++)
            //    {
            //        Sheet.Cells[1, i + 1] = titles[i];
            //    }
            //    // 內容
            //    for (int i = 1; i < rows.Count; i++)
            //    {
            //        //Sheet.Cells[i + 1, 1] = rows[i - 1].Cells[0].Value;
            //        //Sheet.Cells[i + 1, 2] = rows[i - 1].Cells[1].Value;
            //        for (int j = 0; j < 7; j++)
            //        {
            //            Sheet.Cells[i + 1, j + 1] = rows[i - 1].Cells[j].Value;
            //        }
            //    }

            //    // 新增第二個sheet，放到最後一個
            //    // Excel WorkBook
            //    // Excel.Worksheet Sheet2 = book.Sheets.Add(After: book.Sheets[book.Sheets.Count]);
            //    // Sheet2.Name = "1234";
            //    // 把資料塞進 Excel 內
            //    // 標題
            //    // Sheet2.Cells[1, 1] = "身高範圍";
            //    // Sheet2.Cells[1, 2] = "學生統計";
            //    // 內容
            //    /*for (int k = 11; k < 110; k++)
            //    {
            //        Sheet2.Cells[k + 1, 1] = k;
            //        Sheet2.Cells[k + 1, 2] = 2 * k;
            //    }*/
            //    // 新增第三個sheet，放到第一個
            //    // Excel WorkBook
            //    /*Excel.Worksheet Sheet3 = book.Sheets.Add(Before: book.Sheets[1]);
            //    Sheet3.Name = "第0頁";
            //    // 把資料塞進 Excel 內
            //    // 標題
            //    Sheet3.Cells[1, 1] = "我是第一頁";
            //    Sheet3.Cells[1, 2] = "哇哈哈";
            //    // 修改第三頁sheet
            //    book.Sheets[book.Sheets.Count].Cells[4, 5] = "補充說明";*/
            //    // 儲存檔案
            //    book.SaveAs(save.FileName);
            //}
            //catch (Exception err)
            //{
            //    throw;
            //}
            //finally
            //{
            //    xls.Quit();
            //}
            Classes.File file = new Classes.File();
            string fileName = file.readFileName("save", "xlsx");
            DataGridViewRowCollection rows = dataTable.Rows;
            file.write_xlsx(fileName, rows);
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellCollection selRowData = dataTable.Rows[e.RowIndex].Cells;

            string _type = "";
            _type = Convert.ToString(selRowData[2].Value);

            if (_type.Equals("進貨"))
            {
                rbtn_purchase.Checked = true;
            }
            else
            {
                rbtn_sale.Checked = true;
            }

            this.cbox_good.Text = Convert.ToString(selRowData[3].Value);
            this.tbox_price.Text = Convert.ToString(selRowData[4].Value);
            this.tbox_amount.Text = Convert.ToString(selRowData[5].Value);
            this.label5.Text = Convert.ToString(selRowData[0].Value);
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string _name = "";
            int _serial = 0;
            int _stock_type = 0;
            double _price = 0;
            double _number = 0;

            if (rbtn_purchase.Checked == true)
            {
                _stock_type = 0;
            }
            else
            {
                _stock_type = 1;
            }

            // 抓取textbox的資料
            _name = cbox_good.Text;
            _price = Convert.ToDouble(tbox_price.Text);
            _number = Convert.ToDouble(tbox_amount.Text);
            _serial = Convert.ToInt32(label5.Text);

            string sql = @"UPDATE record " +
                      " SET name = '" + _name + "',"
                        + " type = '" + _stock_type.ToString() + "' , "
                        + " price = '" + _price.ToString() + "',"
                        + " number = '" + _number.ToString() + "' "
                        + "   where serial = " + _serial.ToString() + ";";

            DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
            DBConfig.sqlite_cmd.ExecuteNonQuery();
            Show_DB();

        }

        public void write_csv(string fileName)
        {
            // 待驗證
            // 設定儲存excel檔
            SaveFileDialog save = new SaveFileDialog();
            save.InitialDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            save.FileName = "Export_Chart_Data.csv";
            if (save.ShowDialog() != DialogResult.OK) return;
            string strFilePath = save.FileName;
            StringBuilder sbOutput = new StringBuilder();

            for (int i = 0; i < 10; i++)
            {
                string tmp = $"{i}";
                for (int j = 1; j < 10; j++)
                {
                    tmp = $"{tmp},{i}{j}";
                }
                sbOutput.AppendLine(tmp);
            }
            // Create and write the csv file
            System.IO.File.WriteAllText(strFilePath, sbOutput.ToString());
            // To append more lines to the csv file
            System.IO.File.AppendAllText(strFilePath, sbOutput.ToString());
        }

        public void read_csv(string fileName)
        {
            // 待驗證
            System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
            string first_line = sr.ReadLine();
            MessageBox.Show(first_line);
            string[] words = first_line.Split(',');
            MessageBox.Show(words[2]);
        }

        private void 開啟檔案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string fileName = readFileDialog();
            Classes.File file = new Classes.File();
            string fileName = file.readFileName("open", "all");
            DataGridViewRowCollection rows = dataTable.Rows;

            if (fileName == null)
            {
                MessageBox.Show("檔名不得為空");
            } else if (fileName.Contains(".xlsx"))
            {
                file.read_xlsx(fileName, rows);
            } else if (fileName.Contains(".csv"))
            {
                // read_csv(fileName);
            } else if (fileName.Contains(".word"))
            {
                // read_word(fileName);
            } else if (fileName.Contains(".pdf"))
            {
                // read_pdf(fileName);
            }
        }
    }
}