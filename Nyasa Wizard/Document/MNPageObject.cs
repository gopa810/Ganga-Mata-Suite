using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace Nyasa_Wizard
{
    [Serializable()]
    public class MNPageObject
    {
        /// <summary>
        /// Parent page
        /// </summary>
        public MNPage Page { get; set; }

        public MNPageObject ParentObject { get; set; }


        public MNPageObject(MNPage p)
        {
            Page = p;
        }

        public MNDocument Document
        {
            get
            {
                return Page.Document;
            }
        }

        public virtual XmlElement Save(XmlDocument doc)
        {
            XmlElement e1 = doc.CreateElement(this.GetType().Name);
            e1.SetAttribute("selected", Selected.ToString());
            e1.AppendChild(SavePoint(doc, RelativeCenter, "RelativeCenter"));

            return e1;
        }

        public XmlElement SavePoint(XmlDocument doc, Point p, string nodeName)
        {
            XmlElement e2 = doc.CreateElement(nodeName);
            e2.SetAttribute("x", p.X.ToString());
            e2.SetAttribute("y", p.Y.ToString());
            return e2;
        }

        public Point LoadPoint(XmlElement e)
        {
            return new Point(int.Parse(e.GetAttribute("x")), int.Parse(e.GetAttribute("y")));
        }

        public XmlElement SaveColor(XmlDocument doc, Color p, string nodeName)
        {
            XmlElement e2 = doc.CreateElement(nodeName);
            e2.SetAttribute("red", p.R.ToString());
            e2.SetAttribute("green", p.G.ToString());
            e2.SetAttribute("blue", p.B.ToString());
            return e2;
        }

        public Color LoadColor(XmlElement e)
        {
            return Color.FromArgb(int.Parse(e.GetAttribute("red")), 
                int.Parse(e.GetAttribute("green")), int.Parse(e.GetAttribute("blue")));
        }

        public XmlElement SaveSize(XmlDocument doc, Size p, string nodeName)
        {
            XmlElement e2 = doc.CreateElement(nodeName);
            e2.SetAttribute("cx", p.Width.ToString());
            e2.SetAttribute("cy", p.Height.ToString());
            return e2;
        }

        public Size LoadSize(XmlElement e)
        {
            return new Size(int.Parse(e.GetAttribute("cx")), int.Parse(e.GetAttribute("cy")));
        }

        public XmlElement SaveString(XmlDocument doc, string s, string nodeName)
        {
            XmlElement e = doc.CreateElement(nodeName);
            e.InnerText = s;
            return e;
        }

        public string LoadString(XmlElement e)
        {
            return e.InnerText;
        }

        public XmlElement SaveInteger(XmlDocument doc, int i, string nodeName)
        {
            return SaveString(doc, i.ToString(), nodeName);
        }

        public int LoadInteger(XmlElement e)
        {
            return int.Parse(LoadString(e));
        }

        public virtual List<MNPageObject> Load(XmlElement e)
        {
            if (e.HasAttribute("selected"))
                Selected = bool.Parse(e.GetAttribute("selected"));
            foreach (XmlElement e1 in e.ChildNodes)
            {
                if (e1.Name.Equals("RelativeCenter"))
                {
                    RelativeCenter = LoadPoint(e1);
                }
            }

            List<MNPageObject> result = new List<MNPageObject>();
            result.Add(this);
            return result;
        }

        /// <summary>
        /// This is relative point (as if page or image has 1000 x 1000 pt)
        /// </summary>
        public Point RelativeCenter { get; set; }

        public virtual bool Selected { get; set; }

        public Point LogicalToRelative(MNPageContext context, Point p)
        {
            return new Point(p.X * 1000 / context.PageWidth, p.Y * 1000 / context.PageHeight);
        }

        public Point RelativeToLogical(MNPageContext context, Point relPoint)
        {
            return new Point(relPoint.X * context.PageWidth / 1000, relPoint.Y * context.PageHeight / 1000);
        }

        public virtual MNPageObject TestHitLogical(MNPageContext context, Point logicalPoint)
        {
            return null;
        }

        /// <summary>
        /// Move object by specified offset defined in logical coordinate system
        /// </summary>
        /// <param name="offset">Logical offset (in logical coordinate system)</param>
        public virtual void Move(MNPageContext context, System.Drawing.Point offset)
        {
            Point rp = LogicalToRelative(context, offset);
            RelativeCenter = new Point(RelativeCenter.X + rp.X, RelativeCenter.Y + rp.Y);
        }

        public virtual void Paint(MNPageContext context)
        {
        }


        public void PaintSelectionMarks(MNPageContext context, int left, int top, int right, int bottom)
        {
            int markWidth = context.PhysicalToLogical(3);
            context.g.FillRectangle(Brushes.DarkBlue, left - markWidth, top - markWidth, 2 * markWidth, 2 * markWidth);
            context.g.FillRectangle(Brushes.DarkBlue, (left + right) / 2 - markWidth, top - markWidth, 2 * markWidth, 2 * markWidth);
            context.g.FillRectangle(Brushes.DarkBlue, right - markWidth, top - markWidth, 2 * markWidth, 2 * markWidth);
            context.g.FillRectangle(Brushes.DarkBlue, left - markWidth, (top + bottom) / 2 - markWidth, 2 * markWidth, 2 * markWidth);
            context.g.FillRectangle(Brushes.DarkBlue, right - markWidth, (top + bottom) / 2 - markWidth, 2 * markWidth, 2 * markWidth);
            context.g.FillRectangle(Brushes.DarkBlue, left - markWidth, bottom - markWidth, 2 * markWidth, 2 * markWidth);
            context.g.FillRectangle(Brushes.DarkBlue, (left + right) / 2 - markWidth, bottom - markWidth, 2 * markWidth, 2 * markWidth);
            context.g.FillRectangle(Brushes.DarkBlue, right - markWidth, bottom - markWidth, 2 * markWidth, 2 * markWidth);
        }

    }
}
