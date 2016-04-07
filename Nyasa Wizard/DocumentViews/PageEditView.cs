using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Nyasa_Wizard
{
    public partial class PageEditView : UserControl
    {
        public delegate void PageChangedEventHandler(object sender, PageEditViewArguments e);

        public event PageChangedEventHandler PageObjectSelected;

        public event PageChangedEventHandler NewPageRequested;

        public MNDocument Document { get; set; }

        public Point LastUserPoint { get; set; }

        // this is normalized to page size 1000 x 1000 points
        public Point LastRelativePoint { get; set; }

        private MNPage PageData { get; set; }

        private int lastpwidth = -1, lastpheight = -1, lastlwidth = -1, lastlheight = -1;

        private MNPageContext Context = new MNPageContext();


        public MNPage Page
        {
            get
            {
                return PageData;
            }
            set
            {
                PageData = value;
            }
        }

        public PageEditView()
        {
            InitializeComponent();
        }

        private void PageEditView_Paint(object sender, PaintEventArgs e)
        {
            if (PageData == null)
                return;

            RecalculateMatrix();
            e.Graphics.Transform = Context.LastMatrix;

            Context.g = e.Graphics;
            Context.PageWidth = PageData.Document.PageWidth;
            Context.PageHeight = PageData.Document.PageHeight;
            Context.drawSelectionMarks = true;

            /*Debugger.Log(0, "", "Page Log Width: " + context.lwidth + ", Height: " + context.lheight + "\n");
            Debugger.Log(0, "", "Matrix " + LastMatrix.Elements[0] + "," + LastMatrix.Elements[1]
                 + "," + LastMatrix.Elements[2] + "," + LastMatrix.Elements[3]
                  + "," + LastMatrix.Elements[4] + "," + LastMatrix.Elements[5] + "\n");*/

            // now we should use logical coordinates only
            e.Graphics.FillRectangle(Brushes.White, 0, 0, Context.PageWidth, Context.PageHeight);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Context.PageWidth, Context.PageHeight);
            //e.Graphics.DrawLine(Pens.LightGray, 0, 0, Context.PageWidth, Context.PageHeight);
            //e.Graphics.DrawLine(Pens.LightGray, 0, Context.PageHeight, Context.PageWidth, 0);


            PageData.Paint(Context);

        }

        /// <summary>
        /// Returns true if Transformation matrix (LastMatrix) was changed 
        /// and needs to be updated in Graphics context
        /// </summary>
        /// <returns></returns>
        private bool RecalculateMatrix()
        {
            int lwidth = PageData.Document.PageWidth;
            int lheight = PageData.Document.PageHeight;

            int pwidth = ClientSize.Width;
            int pheight = ClientSize.Height;

            Debugger.Log(0, "", "Client Width: " + pwidth + ", Client Height: " + pheight + "\n");

            if (lwidth == lastlwidth && lheight == lastlheight &&
                pwidth == lastpwidth && pheight == lastpheight)
                return false;

            lastlheight = lheight;
            lastlwidth = lwidth;
            lastpwidth = pwidth;
            lastpheight = pheight;

            Context.PageHeight = lheight;
            Context.PageWidth = lwidth;

            double ratio = (double)lwidth / (double)lheight;

            int tmp = Convert.ToInt32(ratio * pheight);
            if (tmp < pwidth)
            {
                // width calculated from ratio and physical height is lower than
                // actual physical width, what means page will be displayed completely
                // so we use this
                double ratioPhysToLog = (double)pheight / lheight;
                int totalLogWidth = Convert.ToInt32(pwidth / ratioPhysToLog);

                Rectangle presentedRect = new Rectangle(-(totalLogWidth - lwidth) / 2, 0, totalLogWidth, lheight);
                Point[] pls = new Point[3];
                pls[0] = new Point(0, 0);
                pls[1] = new Point(pwidth - 1, 0);
                pls[2] = new Point(0, pheight - 1);
                Context.LastMatrix = new Matrix(presentedRect, pls);
                Context.LastInvertMatrix = new Matrix(presentedRect, pls);
                Context.LastInvertMatrix.Invert();
            }
            else
            {
                double ratioPhysToLog = (double)pwidth / lwidth;
                int totalLogHeight = Convert.ToInt32(pheight / ratioPhysToLog);

                Rectangle presentedRect = new Rectangle(0, -(totalLogHeight - lheight) / 2, lwidth, totalLogHeight);
                Point[] pls = new Point[3];
                pls[0] = new Point(0, 0);
                pls[1] = new Point(pwidth - 1, 0);
                pls[2] = new Point(0, pheight - 1);
                Context.LastMatrix = new Matrix(presentedRect, pls);
                Context.LastInvertMatrix = new Matrix(presentedRect, pls);
                Context.LastInvertMatrix.Invert();
            }

            return true;
        }

        private void setPictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PictureSettings dlg = new PictureSettings();
            dlg.Document = Document;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.HasSelectedImage)
                {
                    MNPageImage mpi = new MNPageImage(PageData);
                    mpi.Image = dlg.Image;
                    mpi.RelativeCenter = LastRelativePoint;
                    mpi.Size = mpi.Image.ImageData.Size;
                    mpi.Selected = true;

                    PageData.ClearSelection();
                    PageData.Objects.Add(mpi);

                    Invalidate();

                    PageEditViewArguments args = new PageEditViewArguments();
                    args.PageView = this;
                    args.Page = PageData;
                    args.Document = Document;
                    args.Object = mpi;

                    if (PageObjectSelected != null)
                    {
                        PageObjectSelected.Invoke(this, args);
                    }

                }
            }
        }

        private void insertMantraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Document.Mantras.Count == 0)
                return;

            InsertMantraDialog dlg = new InsertMantraDialog();
            dlg.SetMantras(Document.Mantras);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                MNReferencedMantra selected = dlg.SelectedItem;
                if (selected != null)
                {
                    MNPageMantra pm = new MNPageMantra(PageData);
                    pm.Mantra.Mantra = selected;
                    pm.Mantra.Text = selected.Number + ". " + selected.MantraText;
                    pm.Mantra.ImageCode = 0;
                    pm.Mantra.RelativeCenter = LastRelativePoint;

                    pm.HotSpot.Radius = 30;
                    pm.HotSpot.RelativeCenter = new Point(LastRelativePoint.X + 200, LastRelativePoint.Y + 100);

                    pm.Mantra.Selected = true;

                    PageData.ClearSelection();
                    PageData.Objects.Add(pm);
                    PageData.Objects.Add(pm.Mantra);
                    PageData.Objects.Add(pm.HotSpot);
                    //PageData.Objects.Add(pm);

                    Invalidate();

                    PageEditViewArguments args = new PageEditViewArguments();
                    args.PageView = this;
                    args.Page = PageData;
                    args.Document = Document;
                    args.Object = pm.Mantra;

                    if (PageObjectSelected != null)
                    {
                        PageObjectSelected.Invoke(this, args);
                    }

                }
            }
        }

        private void insertNewPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewPageRequested != null)
            {
                PageEditViewArguments args = new PageEditViewArguments();

                args.Document = Document;
                args.Page = PageData;

                NewPageRequested.Invoke(this, args);
            }
        }

        private void PageEditView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Point clientPoint = new Point(e.X, e.Y);
                LastUserPoint = PointToScreen(clientPoint);
                LastRelativePoint = Context.PhysicalToRelative(clientPoint);
                contextMenuStrip1.Show(LastUserPoint);
            }
        }

        private void pagePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PagePropertiesDialog dlg = new PagePropertiesDialog();

            dlg.PageOrientation = PageData.Document.PageOrientation;
            dlg.PageSize = PageData.Document.PageSize;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PageData.Document.PageOrientation = dlg.PageOrientation;
                PageData.Document.PageSize = dlg.PageSize;

                Invalidate();
            }
        }

        private void PageEditView_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void PageEditView_MouseUp(object sender, MouseEventArgs e)
        {
            if (Context.TrackedObjects.Count > 0)
            {
                foreach (MNPageObject po in Context.TrackedObjects)
                {
                    if (po.Selected)
                        po.Move(Context, Context.TrackedDrawOffset);
                }
                Context.TrackedObjects.Clear();
            }

            Invalidate();
        }

        private void PageEditView_MouseDown(object sender, MouseEventArgs e)
        {
            Point logPoint = Context.PhysicalToLogical(new Point(e.X, e.Y));

            Context.TrackedObjects.Clear();

            foreach (MNPageObject po in PageData.Objects)
            {
                if (po.Selected)
                {
                    MNPageObject testResult = po.TestHitLogical(Context,logPoint);
                    if (testResult != null)
                    {
                        foreach (MNPageObject po2 in PageData.Objects)
                        {
                            if (po.Selected)
                                Context.TrackedObjects.Add(po2);
                        }
                        break;
                    }
                }
            }


            if (Context.TrackedObjects.Count == 0)
            {
                PageData.ClearSelection();
                foreach (MNPageObject po in PageData.Objects)
                {
                    if (!(po is MNPageImage))
                    {
                        MNPageObject testResult = po.TestHitLogical(Context, logPoint);
                        if (testResult != null)
                        {
                            testResult.Selected = true;
                            Context.TrackedObjects.Add(testResult);
                            break;
                        }
                    }
                }
            }

            if (Context.TrackedObjects.Count == 0)
            {
                foreach (MNPageObject po in PageData.Objects)
                {
                    if (po is MNPageImage)
                    {
                        MNPageObject testResult = po.TestHitLogical(Context, logPoint);
                        if (testResult != null)
                        {
                            testResult.Selected = true;
                            Context.TrackedObjects.Add(testResult);
                            break;
                        }
                    }
                }
            }

            if (Context.TrackedObjects.Count > 0)
            {
                if (PageObjectSelected != null)
                {
                    PageEditViewArguments args = new PageEditViewArguments();
                    args.Document = Document;
                    args.Page = PageData;
                    args.Object = Context.TrackedObjects[0];
                    args.PageView = this;
                    PageObjectSelected.Invoke(this, args);
                }
                Context.TrackedStartLogical = logPoint;
                Context.TrackedDrawOffset = Point.Empty;
                Invalidate();
            }
            else
            {
                if (logPoint.X > 0 && logPoint.X < Context.PageWidth &&
                    logPoint.Y > 0 && logPoint.Y < Context.PageHeight)
                {
                    if (PageObjectSelected != null)
                    {
                        PageEditViewArguments args = new PageEditViewArguments();
                        args.Document = Document;
                        args.Page = PageData;
                        args.PageView = this;
                        PageObjectSelected.Invoke(this, args);
                    }
                }
                else
                {
                    if (PageObjectSelected != null)
                    {
                        PageEditViewArguments args = new PageEditViewArguments();
                        args.Document = Document;
                        args.PageView = this;
                        PageObjectSelected.Invoke(this, args);
                    }
                }
            }
        }

        private void PageEditView_MouseMove(object sender, MouseEventArgs e)
        {
            Point logPoint = Context.PhysicalToLogical(new Point(e.X, e.Y));

            Context.TrackedDrawOffset.X = logPoint.X - Context.TrackedStartLogical.X;
            Context.TrackedDrawOffset.Y = logPoint.Y - Context.TrackedStartLogical.Y;

            Invalidate();
        }

        private void insertTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MNPageTextObject pm = new MNPageTextObject(PageData);
            pm.Text = "Text A";
            pm.RelativeCenter = LastRelativePoint;


            PageData.ClearSelection();
            PageData.Objects.Add(pm);

            Invalidate();

            PageEditViewArguments args = new PageEditViewArguments();
            args.PageView = this;
            args.Page = PageData;
            args.Document = Document;
            args.Object = pm;

            if (PageObjectSelected != null)
            {
                PageObjectSelected.Invoke(this, args);
            }

        }

        private void PageEditView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (PageData.HasSelectedObjects())
                {
                    if (MessageBox.Show("Delete selected objects?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        PageData.DeleteSelectedObjects();
                        Invalidate();
                    }
                }
            }
        }

        private void insertLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MNLine line = new MNLine(PageData);
            line.StartPoint.RelativeCenter = LastRelativePoint;
            line.EndPoint.RelativeCenter = new Point(LastRelativePoint.X + 200, LastRelativePoint.Y + 100);

            PageData.ClearSelection();
            PageData.Objects.Add(line.StartPoint);
            PageData.Objects.Add(line.EndPoint);
            PageData.Objects.Add(line);

            Invalidate();

            PageEditViewArguments args = new PageEditViewArguments();
            args.PageView = this;
            args.Page = PageData;
            args.Document = Document;
            args.Object = line;

            if (PageObjectSelected != null)
            {
                PageObjectSelected.Invoke(this, args);
            }

        }
    }
}
