using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Classes
{
    public class File
    {
        public string readFileName(string subName)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "選擇檔案";
            open.InitialDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            open.FileName = "";
            // open.Filter = "所有檔案(*.*) | *.* | xlsx檔案(*.xlsx) | *.xlsx";
            
            if (subName == "xlsx")
            {
                open.Filter = "xlsx檔案(*.xlsx) | *.xlsx";
                //open.FilterIndex = 2;
            } else if (subName == "jpg" || subName == "png")
            {
                open.Filter = "jpg檔案(*.jpg) | *.jpg | png檔案(*.png) | *.jpg";
            } else
            {
                open.Filter = "所有檔案(*.*) | *.*";
                //open.FilterIndex = 1;
            }

            if (open.FileName != null && open.ShowDialog() == DialogResult.OK)
            {
                return open.FileName;
            } else
            {
                return null;
            }

            return null;
        }

        public string saveFileName(string subName)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "儲存檔案";
            save.InitialDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            save.RestoreDirectory = true;
            save.Title = "選擇檔案";
            save.InitialDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            save.FileName = $"{date}.{subName}";
            // save.Filter = "所有檔案(*.*) | *.* Excel檔案(*.xlsx) | *.xlsx";

            if (subName == "xlsx")
            {
                save.Filter = "xlsx檔案(*.xlsx) | *.xlsx";
            } else if (subName == "pdf")
            {
                save.Filter = "pdf檔案(*.pdf) | *.pdf";
            }
            else
            {
                save.Filter = "所有檔案(*.*) | *.*";
            }

            if (save.ShowDialog() != DialogResult.OK) return null;

            return save.FileName;
        }

        public void read_xlsx(string fileName, DataGridViewRowCollection rows)
        {
            Excel.Application xls = null;
            try
            {
                //DataGridViewRowCollection rows = dataTable.Rows;
                // 打開excel
                xls = new Excel.Application();
                // 打開第一個sheet
                // Excel WorkBook
                Excel.Workbook book = xls.Workbooks.Open(fileName);
                Excel.Worksheet Sheet = xls.ActiveSheet;
                // Excel rows, columns count
                Excel.Range range = Sheet.UsedRange;
                // 讀取cell
                // init datatable
                rows.Clear();

                for (int i = 0; i < range.Rows.Count; i++)
                {
                    // cell[1,] 是標題列，需要去掉
                    Object[] row = new Object[range.Columns.Count];

                    for (int j = 0; j < range.Columns.Count; j++)
                    {
                        row[j] = Sheet.Cells[i + 2, j + 1].Value;
                    }

                    rows.Add(row);
                }
                //book.SaveAs(fileName);
                xls.Quit();
            }
            catch (Exception a)
            {
                throw;
            }
            finally
            {
                xls.Quit();
            }
        }

        public void write_xlsx(string fileName, DataGridViewRowCollection rows)
        {
            // DataGridViewRowCollection rows = dataTable.Rows;
            // 設定儲存excel檔
            //SaveFileDialog save = new SaveFileDialog();
            //save.InitialDirectory =
            //Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //save.FileName = "Export_Chart_Data";
            //save.Filter = "*.xlsx | *.xlsx";
            //if (save.ShowDialog() != DialogResult.OK) return;
            // Excel 物件
            //fileName = $"{fileName}";
            Excel.Application xls = null;
            try
            {
                // 打開excel
                xls = new Excel.Application();
                // 新增第一個sheet
                // Excel WorkBook
                Excel.Workbook book = xls.Workbooks.Add();
                //Excel.Worksheet Sheet = (Excel.Worksheet)book.Worksheets[1];
                Excel.Worksheet Sheet = xls.ActiveSheet;
                // 把資料塞進 Excel 內
                // 標題
                string[] titles = new string[] { "編號", "日期", "類別", "名稱", "單價", "數量", "總價" };

                for (int i = 0; i < titles.Length; i++)
                {
                    Sheet.Cells[1, i + 1] = titles[i];
                }
                // 內容
                for (int i = 1; i < rows.Count; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        Sheet.Cells[i + 1, j + 1] = rows[i - 1].Cells[j].Value;
                    }
                }
                // 儲存檔案
                book.SaveAs(fileName);
                xls.Quit();
            }
            catch (Exception err)
            {
                throw;
            }
            finally
            {
                xls.Quit();
            }
        }
        public void delete_file(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    System.IO.File.Delete(fileName);
                } catch (Exception e)
                {
                    MessageBox.Show($"檔案刪除錯誤，{e}");
                }
            } else
            {
                MessageBox.Show($"檔案不存在");
            }
        }
    }
}
