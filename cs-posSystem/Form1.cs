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
using PDF_HorizontalAlignment = iText.Layout.Properties.HorizontalAlignment;
using static System.Net.Mime.MediaTypeNames;
using MathNet.Numerics.Statistics;

namespace cs_posSystem
{
    public partial class Form1 : Form
    {
        // ref: https://dotblogs.com.tw/DavidTalk/2018/06/03/182559
        private float X;//當前窗體的寬度
        private float Y;//當前窗體的高度
        bool isLoaded;  // 是否已設定各控制的尺寸資料到Tag屬性
        // ScottPlot start
        private Crosshair Crosshair;
        // ScottPlot end
        private Classes.File file = new Classes.File();
        public Form1()
        {
            InitializeComponent();
            isLoaded = false;
            Form1_Load(null, null);
            Form1_Shown(null, null);
            //Form1_Resize();

            var plt = formsPlot1.Plot;

            // show login form dialog
            Form2 from_login;
            from_login = new Form2();
            from_login.ShowDialog();

            Load_DB();
            //Show_DB();
            this.label5.Text = index.ToString();

            //ScottPlot start
            Crosshair = formsPlot1.Plot.AddCrosshair(0, 0);
            formsPlot1.Refresh();
            //ScottPlot end

            formsPlot1_MouseLeave(null, null);

            // Set axis limits to control the view
            // (min x, max x, min y, max y)
            plt.SetAxisLimits(0, 100, -25, 25);

            initView();
        }

        private void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    SetTag(con);
            }
        }

        private void SetControls(float newx, float newy, Control cons)
        {
            if (isLoaded)
            {
                //遍歷窗體中的控制項，重新設置控制項的值
                foreach (Control con in cons.Controls)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//獲取控制項的Tag屬性值，並分割後存儲字元串數組
                    float a = System.Convert.ToSingle(mytag[0]) * newx;//根據窗體縮放比例確定控制項的值，寬度
                    con.Width = (int)a;//寬度
                    a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                    con.Height = (int)(a);
                    a = System.Convert.ToSingle(mytag[2]) * newx;//左邊距離
                    con.Left = (int)(a);
                    a = System.Convert.ToSingle(mytag[3]) * newy;//上邊緣距離
                    con.Top = (int)(a);
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字體大小
                    con.Font = new System.Drawing.Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        SetControls(newx, newy, con);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            X = this.Width;//獲取窗體的寬度
            Y = this.Height;//獲取窗體的高度
            isLoaded = true;// 已設定各控制項的尺寸到Tag屬性中
            SetTag(this);//調用方法
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = (this.Height) / Y;
            SetControls(newx, newy, this);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        int index = 1;
        public class DBConfig
        {
            //log.db要放在【bin\Debug底下】      
            public static string dbFile = Application.StartupPath + @"\log.db";
            //public static string dbFile = @"C:\Users\User\Desktop\repo\cs-posSystem\bin\Debug\log.db";
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

            initView();

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
            string fileName = file.saveFileName("xlsx");
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

        private void btn_write_csv_Click(object sender, EventArgs e)
        {
            string fileName = file.saveFileName("csv");
            //MessageBox.Show(fileName);
            DataGridViewRowCollection rows = dataTable.Rows;
            file.write_csv(fileName, rows);
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
            initView();
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
                file.read_csv(fileName, rows);
            } else if (fileName.Contains(".word"))
            {
                // read_word(fileName);
            } else if (fileName.Contains(".pdf"))
            {
                // read_pdf(fileName);
            }
        }

        private void getPlotData(List<string> datas, List<double> double_datas, int group = 3)
        {
            DataGridViewRowCollection rows = dataTable.Rows;
            Dictionary<string, int> map = new Dictionary<string, int>();
            int next = 0;
            string month = DateTime.Now.ToString().Substring(0, 7).Replace("/", "-");

            for (int i = 0; i < rows.Count - 1; i++)
            {
                string date = rows[i].Cells[1].Value.ToString();
                string data = rows[i].Cells[group].Value.ToString();
                double total = Convert.ToDouble(rows[i].Cells[6].Value.ToString());

                /*if (group == 1)
                {
                    // get yyyy-MM-dd
                    data = data.Substring(0, 10);
                }

                if (rows[i].Cells[2].Value.ToString() == "出貨")
                {
                    if (!datas.Contains(data))
                    {
                        map.Add(data, next);
                        datas.Add(data);
                        double_datas.Add(total);
                        next++;
                    }
                    else
                    {
                        double_datas[map[data]] += total;
                    }
                }*/

                if (date.Substring(0, 7) == month)
                {
                    if (group == 1)
                    {
                        // get yyyy-MM-dd
                        data = data.Substring(0, 10);
                    }

                    if (rows[i].Cells[2].Value.ToString() == "出貨")
                    {
                        if (!datas.Contains(data))
                        {
                            map.Add(data, next);
                            datas.Add(data);
                            double_datas.Add(total);
                            next++;
                        }
                        else
                        {
                            double_datas[map[data]] += total;
                        }
                    }
                }
            }
        }

        private void btn_plotPie_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            List<double> totals = new List<double>();
            formsPlot1.Reset();
            getPlotData(names, totals);

            // data 設定條件，需修改
            Classes.plot plot = new Classes.plot();
            double[] arr_totals = totals.ToArray();
            string[] arr_names = names.ToArray();

            plot.pie(formsPlot1.Plot, arr_totals, arr_names);
            formsPlot1.Refresh();
        }

        private void btn_plotBar_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            List<double> totals = new List<double>();
            formsPlot1.Reset();
            getPlotData(names, totals);

            // data 設定條件，需修改
            Classes.plot plot = new Classes.plot();
            double[] arr_totals = totals.ToArray();
            string[] arr_names = names.ToArray();

            plot.bar(formsPlot1.Plot, arr_totals, arr_names);
            formsPlot1.Refresh();
        }

        private void btn_plotRun_Click(object sender, EventArgs e)
        {
            List<string> dates = new List<string>();
            List<double> totals = new List<double>();
            formsPlot1.Reset();
            getPlotData(dates, totals, 1);

            Classes.plot plot = new Classes.plot();
            double[] arr_totals = totals.ToArray();
            string[] arr_names = dates.ToArray();
            double[] arr_dates = new double[arr_names.Length];

            for (int i = 0; i < arr_names.Length; i++)
            {
                arr_dates[i] = Convert.ToDateTime(arr_names[i]).ToOADate();
            }

            plot.run(formsPlot1.Plot, arr_totals, arr_dates);
            
            formsPlot1.Refresh();
            Crosshair = formsPlot1.Plot.AddCrosshair(0, 0);
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

        private void PrintPDF()
        {
            // Set the output dir and file name
            // string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
            DialogResult result = MessageBox.Show("是否需要匯入公司圖片", "匯入圖片", MessageBoxButtons.YesNoCancel);
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            DataGridViewRowCollection rows = dataTable.Rows;
            string[,] table = new string[rows.Count, rows[0].Cells.Count];
            string[] plotList = {
                Application.StartupPath + @"\pie.png",
                Application.StartupPath + @"\bar.png",
                Application.StartupPath + @"\run.png",
            };
            Boolean isAllPlot = false;

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

                if (imgName == null)
                {
                    MessageBox.Show("請選擇圖片");
                    return;
                } else if (imgName == "cancel")
                {
                    return;
                } else
                {
                    itext_pdf.addContentImg(document, imgName, align: PDF_HorizontalAlignment.RIGHT);
                    //itext_pdf.addFixedImg(pdf, imgName, 930, 160);
                }
            }

            // 2. 加文字

            // 2.1. add header
            itext_pdf.addTitle1(document, $"{date} 出貨圖表");
            itext_pdf.addHr(document);
            itext_pdf.addParagraph(document, $"報表日期：{date}", align: TextAlignment.RIGHT);
            itext_pdf.addNote(pdf, text: "便利貼黏貼處");

            try
            {
                foreach (string plot in plotList)
                {
                    if (!file.exists_file(plot))
                    {
                        isAllPlot = true;
                        break;
                    }
                }

                if (isAllPlot)
                {
                    btn_plotPie_Click(null, null);
                    btn_plotBar_Click(null, null);
                    btn_plotRun_Click(null, null);
                }

                foreach (string plot in plotList)
                {
                    itext_pdf.addContentImg(document, plot);
                }
            } catch (Exception e)
            {
                Console.WriteLine($"[Error] Form1.cs addPdf() at line 563 is failed, e = {e}");
                file.delete_file(tempDst);
            }

            itext_pdf.addTitle1(document, $"{date} 出貨報表");
            itext_pdf.addHr(document);
            itext_pdf.addTable(document, table);

            document.Close();
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

        private void showStatic()
        {
            // https://numerics.mathdotnet.com
            List<string> names = new List<string>();
            List<double> totals = new List<double>();
            getPlotData(names, totals);

            double mean = Statistics.Mean(totals);
            double stddiv = Statistics.StandardDeviation(totals);
            double pstddiv = Statistics.PopulationStandardDeviation(totals);
            double variance = Statistics.Variance(totals);
            double median = Statistics.Median(totals);
            double lowerQuartile = Statistics.LowerQuartile(totals);
            double upperQuartile = Statistics.UpperQuartile(totals);
            double interQuartileRange = Statistics.InterquartileRange(totals);
            double min = Statistics.Minimum(totals);
            double max = Statistics.Maximum(totals);

            string _log = $"平均值: {mean}\n" +
                $"樣本標準差: {stddiv}\n" +
                $"樣本標準差: {pstddiv}\n" +
                $"變異數: {variance}\n" +
                $"中位數: {median}\n" +
                $"四分位數（25 %）: {lowerQuartile}\n" +
                $"四分位數（75 %）: {interQuartileRange}\n" +
                $"最小值: {min}\n" +
                $"最大值: {max}";
            richTextBox_static.Text = _log;
        }

        private void initView()
        {
            Show_DB();
            showStatic();

            Bitmap qrCode = file.createQrcode(richTextBox_static.Text.ToString());
            pictureBox_qrcode.Image = qrCode;
        }
    }
}