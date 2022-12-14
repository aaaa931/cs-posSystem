using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using iText.Forms;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Layout;
using PDF_Rectangle = iText.Kernel.Geom.Rectangle;
using PDF_Color = iText.Kernel.Colors.Color;
using static System.Net.Mime.MediaTypeNames;
using iText.Kernel.Pdf.Extgstate;

namespace Classes
{
    public class iText_pdf
    {
        // 標楷體
        private PdfFont font_tr = PdfFontFactory.CreateFont(@"c:/Windows/fonts/kaiu.ttf", PdfEncodings.IDENTITY_H);
        // 正黑體
        //private PdfFont font_msjh = PdfFontFactory.CreateFont(@"./msjh.ttf", PdfEncodings.IDENTITY_H);
        public void addTitle1(Document document, string text, float fontSize = 36, TextAlignment align = TextAlignment.CENTER)
        {
            Paragraph header = new Paragraph(text)
               .SetTextAlignment(align)
               .SetFontSize(fontSize)
               .SetFont(font_tr);
            document.Add(header);
        }
        public void addTitle2(Document document, string text, float fontSize = 24, TextAlignment align = TextAlignment.CENTER)
        {
            Paragraph header = new Paragraph(text)
               .SetTextAlignment(align)
               .SetFontSize(fontSize)
               .SetFont(font_tr);
            document.Add(header);
        }
        public void addHr(Document document)
        {
            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);
        }
        public void addParagraph(
            Document document,
            string text,
            float fontSize = 24,
            TextAlignment align = TextAlignment.JUSTIFIED,
            float marginTop = 0,
            float marginLeft = 0
        )
        {
            Paragraph paragraph = new Paragraph(text).SetFont(font_tr);
            paragraph.SetMarginTop(marginTop);
            paragraph.SetMarginLeft(marginLeft);
            document.Add(paragraph);
        }
        public void addNote(PdfDocument pdf, int page = 1, string text = null, float fontSize = 24, TextAlignment align = TextAlignment.CENTER)
        {
            // 隨便插一段話
            PdfPage page1 = pdf.GetPage(page); // ##設定文字方塊插在哪一頁
            PdfCanvas pdfCanvas1 = new PdfCanvas(page1);
            PDF_Rectangle rectangle = new PDF_Rectangle(100, 700, 100, 100); // ##設定文字方塊的位置與大小
            PDF_Color bgColour = new DeviceRgb(255, 504, 204);  // ##設定文字方塊的顏色
            pdfCanvas1.SaveState()
                    .SetFillColor(bgColour)
                    .Rectangle(rectangle)
                    .Fill()
                    .RestoreState();
            Canvas canvas = new Canvas(pdfCanvas1, rectangle);
            canvas.Add(new Paragraph(text).SetFont(font_tr));  // ##設定文字方塊的文字

            // 4. close content
            canvas.Close();
        }
        public void addFixedImg(PdfDocument pdf, string fileName, float width, float height, int page = 1)
        {
            // canvas pic
            // // 第一種放圖片方式 (隨意放)
            PdfPage newPage = pdf.AddNewPage();  // 新增一頁pdf
            PdfPage page1 = pdf.GetPage(page);  // 取得第一頁pdf的變數
            PdfCanvas pdfCanvas1 = new PdfCanvas(page1); // 新增一塊pdf畫布
                                                         // 新增一個100x100正方形 (y=200, x=700)
            iText.Kernel.Geom.Rectangle rectangle = new iText.Kernel.Geom.Rectangle(200, 700, 100, 100);
            // 新增一個放圖片的畫布, 畫布裡面塞正方形，畫布是放在pdf畫布裡面
            iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas1, rectangle);
            // 產生一張圖片 (從image file裡面讀圖)
            ImageData imageData = ImageDataFactory.Create(fileName);
            // 把圖片轉成iText格式
            iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData);
            // 設定圖片高寬 (option)
            image.SetHeight(height);
            image.SetWidth(width);
            // 把圖片新增到放圖片的畫布上
            canvas.Add(image);
            canvas.Close();
        }

        public void addContentImg(
            Document document,
            string fileName,
            float width,
            float height,
            TextAlignment align = TextAlignment.CENTER,
            float marginTop = 0,
            float marginLeft = 0
        )
        {
            // content: pic
            // Load image from disk
            // 第二種放圖片方式 (把圖片放到pdf的結構裡)
            // 先新增一張圖片，圖片來自於image file
            ImageData imageData = ImageDataFactory.Create(fileName);
            // 把圖片轉成itext格式
            iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData);
            // 設定圖片置中
            image.SetTextAlignment(align);
            // 設定圖片高寬 (option)
            image.SetHeight(height);
            image.SetWidth(width);
            // 設定圖片離左邊多遠 (option)
            image.SetMarginLeft(marginLeft);
            // 設定圖片離上面多遠 (option)
            image.SetMarginTop(marginTop);
            // 把圖片塞到pdf的結構裡
            document.Add(image);
        }

        public void addTable(Document document, string[,] data)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1) + 1;
            Table table = new Table(cols, true);
            // 因為 iText PdfFont 綁定檔案，所以每開一個新檔案就要重宣告 PdfFont
            table.SetFont(font_tr);
            string[] headers = { "編號", "日期", "類別", "名稱", "單價", "數量", "總價" };
            
            foreach (string header in headers)
            {
                if (header == "日期")
                {
                    addTableHeaderCell(table, header, colspan: 2);
                } else
                {
                    addTableHeaderCell(table, header);
                }
            }

            for (int i = 0; i < rows - 1; i++)
            {
                for (int j = 0; j < cols - 1; j++)
                {
                    if (i % 2 == 0)
                    {
                        if (j == 1)
                        {
                            addTableCell(table, data[i, j], colspan: 2, type: true);
                        }
                        else
                        {
                            Console.WriteLine($"i = {i}, j = {j}");
                            addTableCell(table, data[i, j], type: true);
                        }
                    } else
                    {
                        if (j == 1)
                        {
                            addTableCell(table, data[i, j], colspan: 2);
                        }
                        else
                        {
                            Console.WriteLine($"i = {i}, j = {j}");
                            addTableCell(table, data[i, j]);
                        }
                    }
                }
            }

            document.Add(table);
            table.Complete();
        }
        private void addTableHeaderCell(
            Table table, string text,
            VerticalAlignment align = VerticalAlignment.MIDDLE,
            int colspan = 1,
            int rowspan = 1
        )
        {
            PDF_Color color = new DeviceRgb(35, 181, 211);
            table.AddHeaderCell(
                new Cell(rowspan, colspan)
                .SetVerticalAlignment(align)
                .SetBackgroundColor(color)
                .Add(new Paragraph(text))
            );
        }
        private void addTableCell(
            Table table, string text,
            VerticalAlignment align = VerticalAlignment.MIDDLE,
            int colspan = 1,
            int rowspan = 1,
            Boolean type = false
        )
        {
            PDF_Color color = new DeviceRgb(117, 171, 188);

            if (type)
            {
                table.AddCell(
                    new Cell(rowspan, colspan)
                    .SetVerticalAlignment(align)
                    .SetBackgroundColor(color)
                    .Add(new Paragraph(text))
                );
            } else
            {
                table.AddCell(
                    new Cell(rowspan, colspan)
                    .SetVerticalAlignment(align)
                    .Add(new Paragraph(text))
                );
            }
        }

        public void addPageNum(PdfDocument pdfDoc, Document document)
        {
            int n = pdfDoc.GetNumberOfPages();
            for (int i = 1; i <= n; i++)
            {
                document.ShowTextAligned(
                    new Paragraph($"page {i} of {n}")
                    , 806, 595, i, TextAlignment.RIGHT,
                    VerticalAlignment.TOP, 0);
            }
        }
        public void addWaterMark(PdfDocument pdfDoc, Document document)
        {
            iText.Kernel.Geom.Rectangle pageSize;  // 初始化文字方塊
            PdfCanvas canvas; // 初始化畫布
            int n = pdfDoc.GetNumberOfPages(); // 取得pdf的頁數
            for (int i = 1; i <= n; i++)
            {
                PdfFont font = PdfFontFactory.CreateFont(@"c:/Windows/fonts/kaiu.ttf", PdfEncodings.IDENTITY_H);
                PdfPage page = pdfDoc.GetPage(i); // 先拿第i頁pdf
                pageSize = page.GetPageSize(); // 取得該pdf的頁面大小
                canvas = new PdfCanvas(page); // 新增一個畫布

                //Draw page number
                canvas.BeginText()
                    .SetFontAndSize(font, 7)
                    .MoveText(pageSize.GetWidth() / 2 - 7, 10)
                    .ShowText(i.ToString())
                    .ShowText(" of ")
                    .ShowText(n.ToString())
                    .EndText();

                //Draw watermark
                Paragraph p = new Paragraph("極  機  密 \n Confidential").SetFont(font).SetFontSize(60);
                canvas.SaveState();
                PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.2f); // 設定文字透明度
                canvas.SetExtGState(gs1); // 將文字透明度屬性，設進畫布
                                          // 把浮水印塞到文件裡，最後一個變數45是旋轉
                document.ShowTextAligned(p, pageSize.GetWidth() / 2
                                       , pageSize.GetHeight() / 2
                                       , pdfDoc.GetPageNumber(page)
                                        , TextAlignment.CENTER
                                        , VerticalAlignment.MIDDLE, 45);
                canvas.RestoreState();
            }
        }

    }
}
