using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Nyasa_Wizard
{
    public partial class MantrasView : UserControl
    {
        public DataTable table = new DataTable();

        public DataColumn colNum, colMantra, colPart, colHand;

        public MNDocument Document { get; set; }

        public MantrasView()
        {
            InitializeComponent();

            colNum = table.Columns.Add("Number", typeof(string));
            colMantra = table.Columns.Add("Mantra", typeof(string));
            colPart = table.Columns.Add("Part", typeof(string));
            colHand = table.Columns.Add("Hand code", typeof(string));

            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (!ImportMantras(dlg.FileName))
                {
                    MessageBox.Show("Given CSV file does not contain items to import. Please note:\n- File should be UTF8 file\n- every line is imported (first line is not ignored)\n- every line has to have these fields:\n- - number\n- - mantra\n- - touched part\n- - fingers used\n\n");
                }
            }

        }

        public bool ImportMantras(string fileName)
        {
            int importedCount = 0;

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split('\t');
                    if (parts.Length == 4)
                    {

                        MNReferencedMantra rm = new MNReferencedMantra();

                        rm.Number = parts[0];
                        rm.MantraText = parts[1];
                        rm.TouchedPartText = parts[2];
                        rm.HandGestureText = parts[3];

                        Document.Mantras.Add(rm);
                        importedCount++;
                    }
                }

                if (importedCount > 0)
                    RefreshTableContent();
            }

            return (importedCount > 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear the whole table?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                Document.Mantras.Clear();
                RefreshTableContent();
            }
        }

        public void RefreshTableContent()
        {
            table.Rows.Clear();

            foreach (MNReferencedMantra rm in Document.Mantras)
            {
                DataRow dr = table.NewRow();

                dr.SetField<string>(colNum, rm.Number);
                dr.SetField<string>(colMantra, rm.MantraText);
                dr.SetField<string>(colPart, rm.TouchedPartText);
                dr.SetField<string>(colHand, rm.HandGestureText);

                table.Rows.Add(dr);
            }
        }
    }
}
