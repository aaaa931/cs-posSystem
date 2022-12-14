using ExcelDataReader.Log;
using Microsoft.Office.Interop.Word;
using System;
using System.IO;
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
using ScottPlot.Plottable;
using iText.Forms;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;
using PDF_Document = iText.Layout.Document;
using PDF_Paragraph = iText.Layout.Element.Paragraph;
using PDF_Table = iText.Layout.Element.Table;

namespace cs_posSystem
{
    public partial class Form1 : Form
    {
        // ScottPlot start
        private Crosshair Crosshair;
        // ScottPlot end
        public Form1()
        {
            InitializeComponent();

            var plt = formsPlot1.Plot;

            // show login form dialog
            Form2 from_login;
            from_login = new Form2();
            from_login.ShowDialog();

            Load_DB();
            Show_DB();
            this.label5.Text = index.ToString();

            //ScottPlot start
            Crosshair = formsPlot1.Plot.AddCrosshair(0, 0);
            formsPlot1.Refresh();
            //ScottPlot end

            formsPlot1_MouseLeave(null, null);
            //plt.AddSignal(values);

            // Set axis limits to control the view
            // (min x, max x, min y, max y)
            plt.SetAxisLimits(0, 100, -25, 25);
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
            Classes.File file = new Classes.File();
            string fileName = file.saveFileName("xlsx");
            MessageBox.Show(fileName);
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
            string fileName = file.readFileName("all");
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

        private void btn_testPlot_Click(object sender, EventArgs e)
        {
            DataGridViewRowCollection rows = dataTable.Rows;
            DateTime[] dates = new DateTime[rows.Count];
            string[] names = new string[rows.Count];
            double[] prices = new double[rows.Count];
            double[] amounts = new double[rows.Count];
            

            for (int i = 0; i < rows.Count - 1; i++)
            {
                if (rows[i].Cells[2].Value.ToString() == "出貨")
                {
                    names[i] = rows[i].Cells[3].Value.ToString();
                    prices[i] = Convert.ToDouble(rows[i].Cells[4].Value.ToString());
                    amounts[i] = Convert.ToDouble(rows[i].Cells[5].Value.ToString());
                }

                //MessageBox.Show(rows[i].Cells[1].Value.ToString());
                dates[i] = Convert.ToDateTime(rows[i].Cells[1].Value.ToString());
            }

            // data 去重複，需修改
            // test

            //form_plot plot = new form_plot();
            //plot.plot_pie(amounts, names);
            //plot.ShowDialog();
            plot_pie(amounts, names);
        }

        // 滑鼠移動進入圖表時，顯示座標
        private void formsPlot1_MouseEnter(object sender, EventArgs e)
        {
            Crosshair.IsVisible = true;
        }

        // 滑鼠移動時，顯示座標
        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            (double coordinateX, double coordinateY) =
                                     formsPlot1.GetMouseCoordinates();

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            formsPlot1.Refresh(lowQuality: true, skipIfCurrentlyRendering: true);

        }

        // 滑鼠移動離開圖表時，關閉顯示座標
        private void formsPlot1_MouseLeave(object sender, EventArgs e)
        {
            Crosshair.IsVisible = false;
            formsPlot1.Refresh();
        }

        public void plot_pie(double[] data, string[] labels)
        {
            var plt = formsPlot1.Plot;
            var pie = plt.AddPie(data);
            pie.SliceLabels = labels;
            pie.ShowPercentages = true;
            pie.ShowLabels = true;
            pie.Explode = true;
            plt.Legend();

            // mouse event
            formsPlot1_MouseLeave(null, null);
            plt.AddSignal(data);

            // customize the axis labels
            plt.Title($"{date} 出貨圓餅圖");

            // 4. 將統計圖顯示在GUI上面
            formsPlot1.Refresh();
        }

        private void PrintPDF()
        {
            // Set the output dir and file name
            // string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Classes.File file = new Classes.File();
            string fileName = file.saveFileName("pdf");

            if (fileName == null) return;
            
            addPdf(fileName);
            // D12-PDF報表處理 3 - 7 待完成
        }

        private void addPdf(string dst)
        {
            // 1. create pdf
            string tempDst = dst.Replace(".pdf", "_temp.pdf");
            PdfWriter writer = new PdfWriter(tempDst);
            PdfDocument pdf = new PdfDocument(writer);
            PDF_Document document = new PDF_Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(20, 20, 20, 20);
            Classes.iText_pdf itext_pdf = new Classes.iText_pdf();
            Classes.File file = new Classes.File();
            DialogResult result = MessageBox.Show("是否需要匯入圖片", "匯入圖片", MessageBoxButtons.YesNoCancel);
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            DataGridViewRowCollection rows = dataTable.Rows;
            string[,] table = new string[rows.Count, rows[0].Cells.Count];
            //string[] row = new string[rows[0].Cells.Count];

            for (int i = 0; i < rows.Count - 1; i++)
            {
                for (int j = 0; j < rows[i].Cells.Count; j++)
                {
                    Console.WriteLine(rows[i].Cells[j].Value.ToString());
                    table[i, j] = rows[i].Cells[j].Value.ToString();
                }
            }

            if (result == DialogResult.Yes)
            {
                string imgName = file.readFileName("jpg");
                itext_pdf.addContentImg(document, imgName, 930, 160, align: TextAlignment.RIGHT);
                itext_pdf.addFixedImg(pdf, imgName, 930, 160);
            }

            // 2. 加文字

            // 2.1. add header
            itext_pdf.addTitle1(document, $"{date} 存貨報表");
            //itext_pdf.addTitle2(document, "測試標題2");
            itext_pdf.addHr(document);
            itext_pdf.addParagraph(document, $"報表日期：{date}", align: TextAlignment.RIGHT);
            //itext_pdf.addParagraph(document, "根據《今日美國報》報導，美職28日賽後確定了16" +
            //    "支季後賽球隊的對戰席次，戰況膠著的國聯因巨人、費城人都輸球，釀酒人搶" +
            //    "下最後一張晉級門票，他們將作客對上頭號種子道奇，國聯第2種子勇士將迎戰" +
            //    "紅人，第3種子小熊將迎戰今年鹹魚大翻身的馬林魚，4、5種子教士與紅雀正面" +
            //    "對決。", marginTop: 555, marginLeft: 123);
            itext_pdf.addNote(pdf, text: "便利貼黏貼處");
            itext_pdf.addTable(document, table);

            document.Close();
            //pdf.Close();
            //writer.Close();
            // 4. edit existed pdf
            PdfReader reader2 = new PdfReader(tempDst);
            PdfWriter writer2 = new PdfWriter(dst);
            PdfDocument pdfDoc2 = new PdfDocument(reader2, writer2);
            PDF_Document document2 = new PDF_Document(pdfDoc2);

            // 5. add Page numbers
            itext_pdf.addPageNum(pdfDoc2, document2); // 右上角的編碼
            itext_pdf.addWaterMark(pdfDoc2, document2);
            document2.Close();
            file.delete_file(tempDst);

            MessageBox.Show($"匯出 PDF 成功");
        }

        private void btn_testPdf_Click(object sender, EventArgs e)
        {
            PrintPDF();
        }
    }
}