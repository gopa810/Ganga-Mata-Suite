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
    public partial class NyasaMantraProperties : UserControl
    {
        public NyasaMantraProperties()
        {
            InitializeComponent();
        }

        public MNDocument Document { get; set; }
        public MNPage Page { get; set; }
        public MNPageTextWithImage Object { get; set; }
        public PageEditView PageView { get; set; }

        private bool NoUpdate = false;

        public void Set(MNDocument doc, MNPage page, MNPageTextWithImage obj)
        {
            Document = doc;
            Page = page;
            Object = obj;

            richTextBox1.Text = Object.Text;
            richTextBox2.Text = Object.Mantra.TouchedPartText;
            richTextBox3.Text = Object.Mantra.HandGestureText;
            ImageCode = Object.ImageCode;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            Object.Text = richTextBox1.Text;
            Object.TextSize = Size.Empty;
            PageView.Invalidate();
        }

        public int ImageCode
        {
            get
            {
                if (checkBox6.Checked)
                    return 32;
                return (checkBox1.Checked ? 0x01 : 0x0) |
                    (checkBox2.Checked ? 0x02 : 0x0) |
                    (checkBox3.Checked ? 0x04 : 0x0) |
                    (checkBox4.Checked ? 0x08 : 0x0) |
                    (checkBox5.Checked ? 0x10 : 0x0);
            }
            set
            {
                NoUpdate = true;
                if (value == 32)
                {
                    checkBox1.Checked = false;
                    checkBox2.Checked = false;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    checkBox5.Checked = false;
                    checkBox6.Checked = true;
                }
                else
                {
                    checkBox1.Checked = ((value & 0x01) != 0);
                    checkBox2.Checked = ((value & 0x02) != 0);
                    checkBox3.Checked = ((value & 0x04) != 0);
                    checkBox4.Checked = ((value & 0x08) != 0);
                    checkBox5.Checked = ((value & 0x10) != 0);
                    checkBox6.Checked = false;
                }
                NoUpdate = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (NoUpdate) return;
            Object.ImageCode = ImageCode;
            PageView.Invalidate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (NoUpdate) return;
            Object.ImageCode = ImageCode;
            PageView.Invalidate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (NoUpdate) return;
            Object.ImageCode = ImageCode;
            PageView.Invalidate();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (NoUpdate) return;
            Object.ImageCode = ImageCode;
            PageView.Invalidate();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (NoUpdate) return;
            Object.ImageCode = ImageCode;
            PageView.Invalidate();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (NoUpdate) return;
            Object.ImageCode = ImageCode;
            PageView.Invalidate();
        }

    }
}
