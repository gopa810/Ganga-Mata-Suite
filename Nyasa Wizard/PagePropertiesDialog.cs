using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nyasa_Wizard
{
    public partial class PagePropertiesDialog : Form
    {
        public PagePropertiesDialog()
        {
            InitializeComponent();
        }

        public int PageOrientation
        {
            get
            {
                if (radioButton1.Checked) return MNDocument.PO_PORTAIT;
                if (radioButton2.Checked) return MNDocument.PO_LANDSCAPE;
                return MNDocument.PO_PORTAIT;
            }
            set
            {
                radioButton1.Checked = (value == MNDocument.PO_PORTAIT);
                radioButton2.Checked = (value == MNDocument.PO_LANDSCAPE);
            }
        }

        public int PageSize
        {
            get
            {
                if (radioButton3.Checked) return MNDocument.PS_A4;
                if (radioButton4.Checked) return MNDocument.PS_LETTER;
                return MNDocument.PS_A4;
            }
            set
            {
                radioButton3.Checked = (value == MNDocument.PS_A4);
                radioButton4.Checked = (value == MNDocument.PS_LETTER);
            }
        }
    }
}
