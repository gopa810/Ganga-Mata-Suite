using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nyasa_Wizard
{
    public partial class SimpleTextProperties : UserControl
    {
        public SimpleTextProperties()
        {
            InitializeComponent();
        }

        public MNDocument Document { get; set; }
        public MNPage Page { get; set; }
        public MNPageTextObject Object { get; set; }
        public PageEditView PageView { get; set; }

        private bool noupdate = false;


        public void Set(MNDocument doc, MNPage page, MNPageTextObject obj)
        {
            Document = doc;
            Page = page;
            Object = obj;

            noupdate = true;
            richTextBox1.Text = obj.Text;
            numericUpDown1.Value = (obj.FontSize >= 10 ? obj.FontSize : page.DefaultLabelFontSize);
            noupdate = false;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (noupdate) return;
            Object.Text = richTextBox1.Text;
            PageView.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (noupdate) return;
            Object.FontSize = Convert.ToInt32(numericUpDown1.Value);
            PageView.Invalidate();
        }


    }
}
