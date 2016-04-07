using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Nyasa_Wizard
{
    public partial class ImagesListView : UserControl
    {
        public MNDocument Document { get; set; }

        public ImagesListView()
        {
            InitializeComponent();
        }


        public void RefreshImageList()
        {
            listBox1.Items.Clear();
            if (Document != null)
            {
                foreach (MNReferencedImage img in Document.Images)
                {
                    listBox1.Items.Add(img);
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;

            if (Document == null)
                return;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    foreach (string sFileName in dlg.FileNames)
                    {
                        Image loadedImage = Image.FromFile(sFileName);
                        MNReferencedImage ri = new MNReferencedImage();
                        ri.ImageData = loadedImage;
                        ri.Title = Path.GetFileNameWithoutExtension(sFileName);
                        ri.FilePath = sFileName;
                        Document.Images.Add(ri);
                    }
                    RefreshImageList();
                }
                catch (BadImageFormatException bfe)
                {
                    MessageBox.Show("Invalid format of image.\nImage is not loaded.\n\n" + bfe.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during loading of image.\n\n" + ex.Message);
                }

            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Delete images?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    List<MNReferencedImage> imgs = new List<MNReferencedImage>();
                    foreach (MNReferencedImage im in listBox1.SelectedItems)
                    {
                        imgs.Add(im);
                    }
                    foreach (MNReferencedImage im in imgs)
                    {
                        Document.Images.Remove(im);
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = true;
        }
    }
}
