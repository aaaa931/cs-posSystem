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
    public partial class form_word : Form
    {
        public form_word()
        {
            InitializeComponent();
        }

        public form_word(string dir)
        {
            InitializeComponent();
            // 2.2. 打開word檔
            object readOnly = false;
            object visible = true;
            object save = false;
            object fileName = dir;
            object newTemplate = false;
            object docType = 0;
            object missing = Type.Missing;

            Microsoft.Office.Interop.Word._Document document;
            Microsoft.Office.Interop.Word._Application application = new
            Microsoft.Office.Interop.Word.Application()

            { Visible = false };

            document = application.Documents.Open(ref fileName, ref missing, ref readOnly,
            ref missing, ref missing, ref missing,
            ref missing, ref missing, ref missing,
            ref missing, ref missing, ref visible,
            ref missing, ref missing, ref missing,
            ref missing);
            document.ActiveWindow.Selection.WholeStory();
            document.ActiveWindow.Selection.Copy();
            IDataObject dataObject = Clipboard.GetDataObject();

            // 2.3. 將word檔顯示在rich text box
            richTextBox1.Rtf = dataObject.GetData(DataFormats.Rtf).ToString();

            // 2.4. 關掉視窗時，把word也關掉
            application.Quit(ref missing, ref missing, ref missing);
        }
    }
}
