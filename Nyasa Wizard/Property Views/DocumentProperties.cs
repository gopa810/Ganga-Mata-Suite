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
    public partial class DocumentProperties : UserControl
    {
        public DocumentProperties()
        {
            InitializeComponent();
        }

        public MNDocument Document { get; set; }
        public MNPage Page { get; set; }
        public MNPageObject Object { get; set; }
        public PageEditView PageView { get; set; }
        private bool noupdate = false;

        public void Set(MNDocument doc, MNPage page, MNPageObject obj)
        {
            Document = doc;
            Page = page;
            Object = obj;

            noupdate = true;
            comboBox1.SelectedIndex = Document.PageSize;
            comboBox2.SelectedIndex = Document.PageOrientation;
            textBox1.Text = Document.Title;
            numericUpDown1.Value = Convert.ToDecimal(Document.LabelFontSize);
            numericUpDown2.Value = Convert.ToDecimal(Document.SubLabelFontSize);
            noupdate = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Document.Title = textBox1.Text;
            PageView.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Document.PageSize = comboBox1.SelectedIndex;
            PageView.Invalidate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Document.PageOrientation = comboBox2.SelectedIndex;
            PageView.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (noupdate) return;
            Document.LabelFontSize = Convert.ToInt32(numericUpDown1.Value);
            PageView.Invalidate();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (noupdate) return;
            Document.SubLabelFontSize = Convert.ToInt32(numericUpDown2.Value);
            PageView.Invalidate();
        }
    }
}
