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
    public partial class InsertMantraDialog : Form
    {
        public InsertMantraDialog()
        {
            InitializeComponent();
        }

        public void SetMantras(List<MNReferencedMantra> mantras)
        {
            listView1.Items.Clear();

            foreach (MNReferencedMantra man in mantras)
            {
                ListViewItem item = new ListViewItem(man.Number);
                item.SubItems.Add(man.MantraText);
                item.SubItems.Add(man.TouchedPartText);
                item.SubItems.Add(man.HandGestureText);
                item.Tag = man;
                listView1.Items.Add(item);
            }
        }

        public MNReferencedMantra SelectedItem
        {
            get
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView1.SelectedItems[0];
                    MNReferencedMantra rm = item.Tag as MNReferencedMantra;
                    return rm;
                }

                return null;
            }
        }
    }
}
