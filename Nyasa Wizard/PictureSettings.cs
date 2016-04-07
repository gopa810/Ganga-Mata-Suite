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
    public partial class PictureSettings : Form
    {
        public PictureSettings()
        {
            InitializeComponent();
        }

        private MNDocument DocumentRef = null;

        public MNDocument Document
        {
            set
            {
                DocumentRef = value;
                listBox1.Items.Clear();
                foreach (MNReferencedImage ri in DocumentRef.Images)
                {
                    listBox1.Items.Add(ri.Title);
                }
                if (listBox1.Items.Count > 0)
                    listBox1.SelectedIndex = 0;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public bool HasSelectedImage
        {
            get
            {
                return listBox1.SelectedIndex >= 0;
            }
        }

        public MNReferencedImage Image
        {
            get
            {
                if (listBox1.SelectedIndex >= 0)
                {
                    return DocumentRef.Images[listBox1.SelectedIndex];
                }

                return null;
            }
        }
    }
}
