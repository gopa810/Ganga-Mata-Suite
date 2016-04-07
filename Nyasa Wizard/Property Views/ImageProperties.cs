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
    public partial class ImageProperties : UserControl
    {
        public ImageProperties()
        {
            InitializeComponent();
        }
        public MNDocument Document { get; set; }
        public MNPage Page { get; set; }
        public MNPageImage Object { get; set; }
        public PageEditView PageView { get; set; }

        private MNReferencedImage currentImage = null;
        private bool noupdate = false;

        public void Set(MNDocument doc, MNPage page, MNPageImage obj)
        {
            Document = doc;
            Page = page;
            Object = obj;

            comboBox1.Items.Clear();
            int i = 0;
            int selected = -1;
            foreach (MNReferencedImage image in Document.Images)
            {
                comboBox1.Items.Add(image);
                if (image.Title.Equals(Object.Image.Title))
                {
                    selected = i;
                    currentImage = image;
                    noupdate = true;
                    NormalizeTrackValue(image.ImageData.Size.Width * 100 / Object.Size.Width);
                    noupdate = false;
                }
                i++;
            }

            noupdate = true;
            if (selected >= 0)
                comboBox1.SelectedIndex = selected;
            noupdate = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!noupdate && comboBox1.SelectedIndex >= 0)
            {
                MNReferencedImage ri = comboBox1.Items[comboBox1.SelectedIndex] as MNReferencedImage;
                Object.Image = ri;
                Object.Size = ri.ImageData.Size;
                numericUpDown1.Value = 100;
                PageView.Invalidate();
            }
        }

        private void NormalizeTrackValue(int val)
        {
            numericUpDown1.Value = Math.Min(Math.Max((val / 10) * 10, numericUpDown1.Minimum), numericUpDown1.Maximum);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!noupdate && currentImage != null)
            {
                double d = Convert.ToDouble(numericUpDown1.Value) / 100.0;
                Object.Size = new Size(Convert.ToInt32(currentImage.ImageData.Size.Width * d),
                    Convert.ToInt32(currentImage.ImageData.Size.Height * d));
                PageView.Invalidate();
            }
        }

    }
}
