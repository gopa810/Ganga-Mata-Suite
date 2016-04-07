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
    public partial class LineProperties : UserControl
    {
        public LineProperties()
        {
            InitializeComponent();
        }

        public MNDocument Document { get; set; }
        public MNPage Page { get; set; }
        public MNLine Object { get; set; }
        public PageEditView PageView { get; set; }

        private bool noupdate = false;

        public void Set(MNDocument doc, MNPage page, MNLine obj)
        {
            Document = doc;
            Page = page;
            Object = obj;

            comboBox1.Items.Clear();
            comboBox1.Items.Add("none");
            comboBox1.Items.Add("arrow");
            comboBox1.Items.Add("end");

            comboBox2.Items.Clear();
            comboBox2.Items.Add("none");
            comboBox2.Items.Add("arrow");
            comboBox2.Items.Add("end");

            noupdate = true;
            comboBox1.SelectedIndex = obj.StartCap;
            comboBox2.SelectedIndex = obj.EndCap;
            numericUpDown1.Value = Convert.ToDecimal(obj.Width);
            button1.BackColor = obj.LineColor;
            numericUpDown2.Value = Convert.ToDecimal(obj.EndingSize);
            noupdate = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (noupdate) return;

            Object.StartCap = comboBox1.SelectedIndex;
            PageView.Invalidate();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (noupdate) return;

            Object.EndCap = comboBox2.SelectedIndex;
            PageView.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (noupdate) return;

            Object.Width = Convert.ToInt32(numericUpDown1.Value);
            Object.LinePen = null;
            PageView.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (noupdate) return;

            ColorDialog dlg = new ColorDialog();
            dlg.SolidColorOnly = true;
            dlg.Color = button1.BackColor;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                button1.BackColor = dlg.Color;
                Object.LineColor = dlg.Color;
                Object.LinePen = null;
                PageView.Invalidate();
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (noupdate) return;

            Object.EndingSize = Convert.ToInt32(numericUpDown2.Value);
            PageView.Invalidate();
        }

    }
}
