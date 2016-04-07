using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace Nyasa_Wizard
{
    public class MNPageTextObject: MNPageObject
    {
        public string Text { get; set; }

        public int FontSize { get; set; }

        public Size TextSize { get; set; }

        public MNPageTextObject(MNPage p): base(p)
        {
        }

        public override List<MNPageObject> Load(XmlElement e)
        {
            List < MNPageObject >  list = base.Load(e);
            foreach (XmlElement e1 in e.ChildNodes)
            {
                if (e1.Name == "Text")
                {
                    Text = e1.InnerText;
                }
                else if (e1.Name == "TextSize")
                {
                    TextSize = LoadSize(e1);
                }
                else if (e1.Name == "FontSize")
                {
                    FontSize = LoadInteger(e1);
                }
            }
            return list;
        }

        public override XmlElement Save(XmlDocument doc)
        {
            XmlElement e = base.Save(doc);

            XmlElement e1 = doc.CreateElement("Text");
            e1.InnerText = Text;
            e.AppendChild(e1);

            e.AppendChild(SaveSize(doc, TextSize, "TextSize"));

            e.AppendChild(SaveInteger(doc, FontSize, "FontSize"));

            return e;
        }

        public Rectangle LogicalRectangle(MNPageContext context)
        {
            Point p = new Point(RelativeCenter.X * context.PageWidth / 1000, RelativeCenter.Y * context.PageHeight / 1000);
            Rectangle rect = new Rectangle(p.X - TextSize.Width / 2, p.Y - TextSize.Height / 2, TextSize.Width, TextSize.Height);
            return rect;
        }


        public override MNPageObject TestHitLogical(MNPageContext context, Point logicalPoint)
        {
            return (LogicalRectangle(context).Contains(logicalPoint) ? this : null);
        }

        public Font FontOfThisText
        {
            get
            {
                if (FontSize < 10)
                    return Page.DefaultLabelFont;
                return MNDocument.FontOfSize(FontSize);
            }
        }

        public override void Paint(MNPageContext context)
        {
            SizeF sf = context.g.MeasureString(Text, FontOfThisText);
            TextSize = new Size((int)sf.Width, (int)sf.Height);
            Point pt = RelativeToLogical(context, RelativeCenter);

            if (Selected && context.TrackedObjects.Count > 0)
                pt.Offset(context.TrackedDrawOffset);

            context.g.DrawString(Text, FontOfThisText, Brushes.Black, pt.X - TextSize.Width / 2,
                pt.Y - TextSize.Height / 2);

            if (Selected && context.drawSelectionMarks)
            {
                PaintSelectionMarks(context, pt.X - TextSize.Width / 2, pt.Y - TextSize.Height / 2,
                    pt.X + TextSize.Width / 2, pt.Y + TextSize.Height / 2);
            }
        }
    }
}
