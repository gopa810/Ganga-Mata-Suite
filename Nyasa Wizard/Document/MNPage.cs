using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;

namespace Nyasa_Wizard
{
    [Serializable()]
    public class MNPage
    {
        public MNDocument Document { get; set; }

        public string Title { get; set; }

        public int PageIndex { get; set; }

        public Size DefaultTextImageSize { get; set; }

        [NonSerialized()]
        public TreeNode TreeNode = null;

        /// <summary>
        /// local value for this page
        /// </summary>
        private int def_font_size = -1;

        /// <summary>
        /// Wrapper property for label size on this page
        /// if font size is -1 or so, then font size for document is used
        /// </summary>
        public int DefaultLabelFontSize
        {
            get
            {
                if (def_font_size < 10)
                    return (int)Document.LabelFontSize;
                return def_font_size;
            }
            set
            {
                def_font_size = (value == (int)Document.LabelFontSize ? -1 : value);
            }
        }

        public Font DefaultLabelFont
        {
            get
            {
                if (DefaultLabelFontSize < 10)
                    return Document.LabelFont;
                return MNDocument.FontOfSize(DefaultLabelFontSize);
            }
        }

        public Font DefaultSubLabelFont
        {
            get
            {
                return Document.SubLabelFont;
            }
        }

        public List<MNPageObject> Objects = new List<MNPageObject>();

        public MNPage(MNDocument doc)
        {
            Document = doc;
            DefaultTextImageSize = new Size(180,180);
            DefaultLabelFontSize = -1;
        }

        public bool HasSelectedObjects()
        {
            foreach (MNPageObject po in Objects)
            {
                if (po.Selected)
                    return true;
            }

            return false;
        }

        public void ClearSelection()
        {
            foreach (MNPageObject item in Objects)
            {
                item.Selected = false;
            }
        }

        public void DeleteSelectedObjects()
        {
            List<MNPageObject> objectsForDelete = new List<MNPageObject>();
            HashSet<MNPageObject> selectedParentObjects = new HashSet<MNPageObject>();

            foreach (MNPageObject item in Objects)
            {
                if (item.Selected)
                {
                    if (item.ParentObject == null)
                    {
                        objectsForDelete.Add(item);
                    }
                    else
                    {
                        if (!selectedParentObjects.Contains(item.ParentObject))
                        {
                            objectsForDelete.Add(item.ParentObject);
                            selectedParentObjects.Add(item.ParentObject);
                        }
                    }
                }
            }

            foreach (MNPageObject item in Objects)
            {
                try
                {
                    if (selectedParentObjects.Contains(item.ParentObject))
                    {
                        objectsForDelete.Add(item);
                    }
                }
                catch { }
            }

            // actual delete
            foreach (MNPageObject item in objectsForDelete)
            {
                try {
                    Objects.Remove(item);
                }
                catch {
                }
            }
        }

        public void Paint(MNPageContext context)
        {
            try
            {
                Graphics g = context.g;


                foreach (MNPageObject po in this.Objects)
                {
                    if (po is MNPageImage)
                        po.Paint(context);
                }

                foreach (MNPageObject po in this.Objects)
                {
                    if (!(po is MNPageImage))
                        po.Paint(context);
                }

                // drawing header for page
                SizeF dhs = g.MeasureString(Document.Title, Document.DocumentHeaderFont);
                SizeF phs = g.MeasureString(this.Title, Document.PageHeaderFont);
                float headerBottom = 160 + dhs.Height + phs.Height;
                g.DrawString(Document.Title, Document.DocumentHeaderFont, Brushes.Black, new PointF(120, 120));
                g.DrawString(this.Title, Document.PageHeaderFont, Brushes.DarkGray, new PointF(120, 140 + dhs.Height));
                g.DrawLine(Pens.Black, 120, headerBottom, context.PageWidth - 120, headerBottom);

                string pageNumber = "Page " + PageIndex;
                phs = g.MeasureString(pageNumber, Document.PageHeaderFont);
                g.DrawString(pageNumber, Document.PageHeaderFont, Brushes.Black, 
                    new PointF(context.PageWidth - 120 - phs.Width, headerBottom - 20 - phs.Height));

            }
            catch (Exception ex)
            {
                Debugger.Log(0, "", ex.StackTrace);
                Debugger.Log(0, "", "\n\n");
            }
        }


        public XmlElement Save(XmlDocument doc)
        {
            XmlElement e = doc.CreateElement("page");

            e.SetAttribute("idx", PageIndex.ToString());
            e.SetAttribute("title", Title);
            e.SetAttribute("dlfs", DefaultLabelFontSize.ToString());
            foreach (MNPageObject po in Objects)
            {
                if (po.ParentObject != null) continue;
                e.AppendChild(po.Save(doc));
            }
            return e;
        }

        public void Load(XmlElement e)
        {
            PageIndex = int.Parse(e.GetAttribute("idx"));
            Title = e.GetAttribute("title");
            DefaultLabelFontSize = int.Parse(e.GetAttribute("dlfs"));
            foreach (XmlElement e1 in e.ChildNodes)
            {
                MNPageObject po = null;

                try
                {
                    // creating MNPageImage, MNPagePoint, etc... object from class' name
                    string typeName = this.GetType().Namespace + "." + e1.Name;
                    Type t = Type.GetType(typeName);
                    object instance = Activator.CreateInstance(t, this);
                    if (instance is MNPageObject)
                        po = (MNPageObject)instance;
                }
                catch
                {
                    po = null;
                }

/*                if (e1.Name.Equals("MNPageImage"))
                {
                    po = new MNPageImage(this);
                }
                else if (e1.Name.Equals("MNPagePoint"))
                {
                    po = new MNPagePoint(this);
                }
                else if (e1.Name.Equals("MNPageMantra"))
                {
                    po = new MNPageMantra(this);
                }
                else if (e1.Name.Equals("MNPageTextObject"))
                {
                    po = new MNPageTextObject(this);
                }
                else if (e1.Name.Equals("MNPageTextWithImage"))
                {
                    po = new MNPageTextWithImage(this);
                }
                else if (e1.Name.Equals("MNLine"))
                {
                    po = new MNLine(this);
                }*/

                if (po != null)
                {
                    po.Page = this;
                    List<MNPageObject> list = po.Load(e1);
                    Objects.AddRange(list);
                }
            }
        }
    }
}
