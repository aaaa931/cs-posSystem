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
    public partial class form_pdf : Form
    {
        public form_pdf()
        {
            InitializeComponent();
        }

        public form_pdf(string dir)
        {
            // get dir to open pdf file
            InitializeComponent();
            axAcroPDF1.LoadFile(dir);
        }
    }
}
