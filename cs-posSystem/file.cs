using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cs_posSystem
{
    public class file
    {
        public string name { get; }
        public file (string type)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "選擇檔案";
            open.InitialDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            open.Filter = "所有檔案(*.*) | *.*";

            if (open.FileName != null && open.ShowDialog() == DialogResult.OK)
            {
                name = open.FileName;
            }

            name = null;
        }
    }
}
