using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Nyasa_Wizard
{
    public partial class Form1 : Form
    {
        MNDocument document = new MNDocument();

        public string CurrentFileName { get; set; }

        private DocumentProperties pvDocument = null;
        private ImageProperties pvImage = null;
        private NyasaMantraProperties pvNyasaMantra = null;
        private PageProperties pvPage = null;
        private SimpleTextProperties pvText = null;
        private LineProperties pvLine = null;

        private int printedPageCurrent = 0;
        private MNPageContext printContext = null;

        public Form1()
        {
            InitializeComponent();

            InitTreeViewWithDocument();

            AssignDocumentToPages();

        }

        private void AssignDocumentToPages()
        {
            mantrasView1.Document = document;
            imagesListView1.Document = document;
            pageEditView1.Document = document;
        }

        private void HideAllSubpanels()
        {
            mantrasView1.Visible = false;
            imagesListView1.Visible = false;
            pageEditView1.Visible = false;
        }

        public void InitTreeViewWithDocument()
        {
            HideAllSubpanels();

            treeView1.Nodes.Clear();

            TreeNode tn1;
            TreeNode tn = treeView1.Nodes.Add("Images");
            tn.Tag = "images";

            tn = treeView1.Nodes.Add("Mantras");
            tn.Tag = "mantras";

            tn = treeView1.Nodes.Add("Pages");

            int i = 1;
            foreach (MNPage page in document.Pages)
            {
                tn1 = tn.Nodes.Add(page.Title);
                tn1.Tag = page;
                page.TreeNode = tn1;
                i++;
            }

            treeView1.ExpandAll();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
                return;
            if (e.Node != null && e.Node.Tag != null)
            {
                HideAllSubpanels();

                if (e.Node.Tag is string)
                {
                    string tag = e.Node.Tag.ToString();

                    if (tag == "images")
                    {
                        imagesListView1.Dock = DockStyle.Fill;
                        imagesListView1.Visible = true;
                        imagesListView1.Document = document;
                        imagesListView1.RefreshImageList();
                    }
                    else if (tag == "mantras")
                    {
                        mantrasView1.Dock = DockStyle.Fill;
                        mantrasView1.Document = document;
                        mantrasView1.Visible = true;
                        mantrasView1.RefreshTableContent();
                    }
                }
                else if (e.Node.Tag is MNPage)
                {
                    int pageNo = (e.Node.Tag as MNPage).PageIndex;
                    if (pageNo > 0 && pageNo <= document.Pages.Count)
                    {
                        pageEditView1.Dock = DockStyle.Fill;
                        pageEditView1.Visible = true;
                        pageEditView1.Page = document.Pages[pageNo - 1];
                        pageEditView1.Document = document;
                        pageEditView1.Invalidate();
                    }
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);

            document = new MNDocument();
            AssignDocumentToPages();

            InitTreeViewWithDocument();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Nyasa Wizard Files (*.nwf)|*.nwf||";
            dlg.FilterIndex = 1;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                document = MNDocument.Load(dlg.FileName);
                CurrentFileName = dlg.FileName;
                if (document == null)
                {
                    MessageBox.Show("File could not be loaded");
                    document = new MNDocument();
                }

                AssignDocumentToPages();

                InitTreeViewWithDocument();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentFileName == null || CurrentFileName.Length == 0)
            {
                if (document != null && document.HasContent())
                    saveAsToolStripMenuItem_Click(sender, e);
                else
                    return;
            }

            XmlDocument doc = new XmlDocument();
            document.Save(doc);
            doc.Save(CurrentFileName);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".nwf";
            dlg.Filter = "Nyasa Wizard Files (*.nwf)|*.nwf||";
            dlg.FilterIndex = 1;

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CurrentFileName = dlg.FileName;
                XmlDocument doc = new XmlDocument();
                document.Save(doc);
                doc.Save(CurrentFileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pageEditView1_PageObjectSelected(object sender, PageEditViewArguments e)
        {
            foreach (Control c in splitContainer2.Panel2.Controls)
            {
                c.Visible = false;
            }

            if (e.Object != null)
            {
                if (e.Object is MNPageImage)
                {
                    if (pvImage == null)
                    {
                        pvImage = new Nyasa_Wizard.ImageProperties();
                        pvImage.Size = new Size(100, 100);
                        pvImage.Location = new Point(0, 0);
                        pvImage.Name = "pvImage";
                        pvImage.TabIndex = splitContainer2.Panel2.Controls.Count + 1;
                        pvImage.Visible = false;
                        splitContainer2.Panel2.Controls.Add(pvImage);
                        pvImage.Dock = DockStyle.Fill;
                    }

                    pvImage.Visible = true;
                    pvImage.PageView = e.PageView;
                    pvImage.Set(e.Document, e.Page, e.Object as MNPageImage);
                }
                else if (e.Object is MNPageTextWithImage)
                {
                    if (pvNyasaMantra == null)
                    {
                        pvNyasaMantra = new Nyasa_Wizard.NyasaMantraProperties();
                        pvNyasaMantra.Size = new Size(100, 100);
                        pvNyasaMantra.Location = new Point(0, 0);
                        pvNyasaMantra.Name = "pvNyasa";
                        pvNyasaMantra.TabIndex = splitContainer2.Panel2.Controls.Count + 1;
                        pvNyasaMantra.Visible = false;
                        splitContainer2.Panel2.Controls.Add(pvNyasaMantra);
                        pvNyasaMantra.Dock = DockStyle.Fill;
                    }

                    pvNyasaMantra.Visible = true;
                    pvNyasaMantra.PageView = e.PageView;
                    pvNyasaMantra.Set(e.Document, e.Page, e.Object as MNPageTextWithImage);
                }
                else if (e.Object is MNPageTextObject)
                {
                    if (pvText == null)
                    {
                        pvText = new Nyasa_Wizard.SimpleTextProperties();
                        pvText.Size = new Size(100, 100);
                        pvText.Location = new Point(0, 0);
                        pvText.Name = "pvText";
                        pvText.TabIndex = splitContainer2.Panel2.Controls.Count + 1;
                        pvText.Visible = false;
                        splitContainer2.Panel2.Controls.Add(pvText);
                        pvText.Dock = DockStyle.Fill;
                    }

                    pvText.Visible = true;
                    pvText.PageView = e.PageView;
                    pvText.Set(e.Document, e.Page, e.Object as MNPageTextObject);
                }
                else if (e.Object is MNLine)
                {
                    if (pvLine == null)
                    {
                        pvLine = new Nyasa_Wizard.LineProperties();
                        pvLine.Size = new Size(100, 100);
                        pvLine.Location = new Point(0, 0);
                        pvLine.Name = "pvLine";
                        pvLine.TabIndex = splitContainer2.Panel2.Controls.Count + 1;
                        pvLine.Visible = false;
                        splitContainer2.Panel2.Controls.Add(pvLine);
                        pvLine.Dock = DockStyle.Fill;
                    }

                    pvLine.Visible = true;
                    pvLine.PageView = e.PageView;
                    pvLine.Set(e.Document, e.Page, e.Object as MNLine);
                }
                else if (e.Object is MNPagePoint)
                {
                    MNPagePoint pp = e.Object as MNPagePoint;

                    if (pp.EditParentProperties)
                    {
                        e.Object = pp.ParentObject;
                        pageEditView1_PageObjectSelected(sender, e);
                    }

                }
            }
            else if (e.Page != null)
            {
                if (pvPage == null)
                {
                    pvPage = new Nyasa_Wizard.PageProperties();
                    pvPage.Size = new Size(100, 100);
                    pvPage.Location = new Point(0, 0);
                    pvPage.Name = "pvPage";
                    pvPage.TabIndex = splitContainer2.Panel2.Controls.Count + 1;
                    pvPage.Visible = false;
                    splitContainer2.Panel2.Controls.Add(pvPage);
                    pvPage.Dock = DockStyle.Fill;
                }

                pvPage.Visible = true;
                pvPage.PageView = e.PageView;
                pvPage.Set(e.Document, e.Page, null);

            }
            else if (e.Document != null)
            {
                if (pvDocument == null)
                {
                    pvDocument = new Nyasa_Wizard.DocumentProperties();
                    pvDocument.Size = new Size(100, 100);
                    pvDocument.Location = new Point(0, 0);
                    pvDocument.Name = "pvDocument";
                    pvDocument.TabIndex = splitContainer2.Panel2.Controls.Count + 1;
                    pvDocument.Visible = false;
                    splitContainer2.Panel2.Controls.Add(pvDocument);
                    pvDocument.Dock = DockStyle.Fill;
                }

                pvDocument.Visible = true;
                pvDocument.PageView = e.PageView;
                pvDocument.Set(e.Document, null, null);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();

            if (pd.ShowDialog() != DialogResult.OK)
                return;

            printedPageCurrent = 0;
            printDocument1.DocumentName = document.Title;
            printDocument1.PrinterSettings = pd.PrinterSettings;

            if (printContext == null)
            {
                printContext = new MNPageContext();

                printContext.drawSelectionMarks = false;
                printContext.TrackedObjects = new List<MNPageObject>();
            }

            printContext.LastMatrix = null;

            printDocument1.Print();

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (printContext.LastMatrix == null)
            {
                printContext.PageHeight = document.PageHeight;
                printContext.PageWidth = document.PageWidth;
                Rectangle presentedRect = new Rectangle(0, 0, printContext.PageWidth, printContext.PageHeight);
                Point[] pls = new Point[3];
                pls[0] = new Point(0, 0);
                pls[1] = new Point(e.PageBounds.Width - 1, 0);
                pls[2] = new Point(0, e.PageBounds.Height - 1);
                printContext.LastMatrix = new System.Drawing.Drawing2D.Matrix(presentedRect, pls);
                printContext.LastInvertMatrix = new System.Drawing.Drawing2D.Matrix(presentedRect, pls);
                printContext.LastInvertMatrix.Invert();
            }

            printContext.g = e.Graphics;
            e.Graphics.Transform = printContext.LastMatrix;

            document.Pages[printedPageCurrent].Paint(printContext);
            printedPageCurrent++;

            e.HasMorePages = (printedPageCurrent < document.Pages.Count);
        }

        private void pageEditView1_NewPageRequested(object sender, PageEditViewArguments e)
        {
            document.InsertPageAfter(e.Page);
            InitTreeViewWithDocument();
        }
    }
}
