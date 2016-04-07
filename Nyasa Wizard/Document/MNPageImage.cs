using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace Nyasa_Wizard
{
    [Serializable()]
    public class MNPageImage: MNPageObject
    {
        public MNReferencedImage Image { get; set; }

        // actual size in pixels
        public Size Size { get; set; }

        public MNPageImage(MNPage p)
            : base(p)
        {
        }

        public override List<MNPageObject> Load(XmlElement e)
        {
            List<MNPageObject> list = base.Load(e);
            foreach (XmlElement e1 in e.ChildNodes)
            {
                if (e1.Name.Equals("size")) Size = LoadSize(e1);
            }
            Image = Document.FindImage(e.GetAttribute("imageName"));
            return list;
        }

        public override XmlElement Save(XmlDocument doc)
        {
            XmlElement e = base.Save(doc);
            e.AppendChild(SaveSize(doc, Size, "size"));
            e.SetAttribute("imageName", Image.Title);
            return e;
        }

        public Rectangle LogicalRectangle(MNPageContext context)
        {
            Point p = new Point(RelativeCenter.X * context.PageWidth / 1000, RelativeCenter.Y * context.PageHeight / 1000);
            Rectangle rect = new Rectangle(p.X - Size.Width / 2, p.Y - Size.Height / 2, Size.Width, Size.Height);
            return rect;
        }

        public override MNPageObject TestHitLogical(MNPageContext context, Point logicalPoint)
        {
            if (LogicalRectangle(context).Contains(logicalPoint))
                return this;
            return null;
        }

        public override void Paint(MNPageContext context)
        {
            Graphics g = context.g;
            MNPageImage pi = this;
            Point center = new Point(pi.RelativeCenter.X * context.PageWidth / 1000, pi.RelativeCenter.Y * context.PageHeight / 1000);
            Rectangle rect = new Rectangle(center.X - pi.Size.Width / 2, center.Y - pi.Size.Height / 2, pi.Size.Width, pi.Size.Height);

            if (pi.Selected && context.TrackedObjects.Count > 0)
            {
                rect.Offset(context.TrackedDrawOffset);
                center.Offset(context.TrackedDrawOffset);
            }
            g.DrawImage(pi.Image.ImageData, rect);

            if (pi.Selected && context.drawSelectionMarks)
            {
                PaintSelectionMarks(context, rect.Left, rect.Top, rect.Right, rect.Bottom);
            }
        }
    }
}
