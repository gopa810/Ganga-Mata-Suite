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
    public partial class PageProperties : UserControl
    {
        public PageProperties()
        {
            InitializeComponent();
        }

        public MNDocument Document { get; set; }
        public MNPage Page { get; set; }
        public MNPageObject Object { get; set; }
        public PageEditView PageView { get; set; }

        public static int[] FontSizes = { 40, 50, 60, 70, 80, 90, 100, 120, 140 };
        public bool noupdate = false;

        public void Set(MNDocument doc, MNPage page, MNPageObject obj)
        {
            Document = doc;
            Page = page;
            Object = obj;

            noupdate = true;
            numericUpDown1.Value = Page.DefaultLabelFontSize;
            textBox2.Text = Page.Title;
            noupdate = false;
        }

        private int IndexOfSize(int size)
        {
            for (int i = FontSizes.Length - 1; i >= 0; i--)
            {
                if (FontSizes[i] <= size)
                    return i;
            }
            return 0;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (noupdate) return;
            Page.Title = textBox2.Text;
            if (Page.TreeNode != null)
                Page.TreeNode.Text = textBox2.Text;
            PageView.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (noupdate) return;
            int FontSize = Convert.ToInt32(numericUpDown1.Value);
            Page.DefaultLabelFontSize = FontSize;
            foreach (MNPageObject po in Page.Objects)
            {
                if (po is MNPageTextWithImage)
                {
                    (po as MNPageTextWithImage).TextSize = Size.Empty;
                }
            }
            PageView.Invalidate();
        }
    }
}
