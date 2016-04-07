using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;

namespace Nyasa_Wizard
{
    public class MNLine: MNPageObject
    {
        public MNPagePoint StartPoint { get; set; }
        public MNPagePoint EndPoint { get; set; }

        public int StartCap { get; set; }
        public int EndCap { get; set; }
        public int DashType { get; set; }
        public int Width { get; set; }
        public Color LineColor { get; set; }
        public int EndingSize { get; set; }

        public Pen LinePen { get; set; }

        public MNLine(MNPage p)
            : base(p)
        {
            StartPoint = new MNPagePoint(p);
            StartPoint.Radius = 0;
            StartPoint.EditParentProperties = true;
            StartPoint.ParentObject = this;

            EndPoint = new MNPagePoint(p);
            EndPoint.Radius = 0;
            EndPoint.EditParentProperties = true;
            EndPoint.ParentObject = this;

            StartCap = 0;
            EndCap = 0;
            DashType = 0;
            Width = 1;
            EndingSize = 30;
            LineColor = Color.Black;

            LinePen = null;
        }

        public override List<MNPageObject> Load(XmlElement e)
        {
            List < MNPageObject > list =  base.Load(e);

            foreach (XmlElement e1 in e.ChildNodes)
            {
                switch (e1.Name)
                {
                    case "StartCap": StartCap = LoadInteger(e1); break;
                    case "EndCap": EndCap = LoadInteger(e1); break;
                    case "EndingSize": EndingSize = LoadInteger(e1); break;
                    case "DashType": DashType = LoadInteger(e1); break;
                    case "Width": Width = LoadInteger(e1); break;
                    case "LineColor": LineColor = LoadColor(e1); break;
                    case "StartPoint": StartPoint = new MNPagePoint(Page);
                        StartPoint.Load(e1.ChildNodes[0] as XmlElement);
                        break;
                    case "EndPoint": EndPoint = new MNPagePoint(Page);
                        EndPoint.Load(e1.ChildNodes[0] as XmlElement);
                        break;
                    default:
                        break;
                }
            }

            list.Add(StartPoint);
            list.Add(EndPoint);

            return list;
        }

        public override XmlElement Save(XmlDocument doc)
        {
            XmlElement e = base.Save(doc);
            XmlElement d = doc.CreateElement("StartPoint");
            e.AppendChild(d);
            d.AppendChild(StartPoint.Save(doc));

            d = doc.CreateElement("EndPoint");
            e.AppendChild(d);
            d.AppendChild(EndPoint.Save(doc));

            e.AppendChild(SaveInteger(doc, StartCap, "StartCap"));
            e.AppendChild(SaveInteger(doc, EndCap, "EndCap"));
            e.AppendChild(SaveInteger(doc, DashType, "DashType"));
            e.AppendChild(SaveInteger(doc, Width, "Width"));
            e.AppendChild(SaveColor(doc, LineColor, "LineColor"));
            e.AppendChild(SaveInteger(doc, EndingSize, "EndingSize"));
            return e;
        }

        public override void Paint(MNPageContext context)
        {
            Point A = StartPoint.GetLogicalCenter(context);
            Point B = EndPoint.GetLogicalCenter(context);

            if (StartPoint.Selected && context.TrackedObjects.Count > 0)
                A.Offset(context.TrackedDrawOffset);

            if (EndPoint.Selected && context.TrackedObjects.Count > 0)
                B.Offset(context.TrackedDrawOffset);

            if (LinePen == null)
            {
                LinePen = new Pen(LineColor);
                LinePen.Width = Width;
            }

            context.g.DrawLine(LinePen, A, B);

            switch (StartCap)
            {
                case 0: break;
                case 1: PaintArrow(context.g, A, B); break;
                case 2: PaintEnd(context.g, A, B); break;
                default: break;
            }

            switch (EndCap)
            {
                case 0: break;
                case 1: PaintArrow(context.g, B, A); break;
                case 2: PaintEnd(context.g, B, A); break;
                default: break;
            }


            base.Paint(context);
        }

        public void PaintArrow(Graphics g, Point A, Point B)
        {
            double cx = B.X - A.X;
            double cy = B.Y - A.Y;
            double d = Math.Sqrt(cx * cx + cy * cy);
            cx /= d;
            cy /= d;
            cx *= EndingSize;
            cy *= EndingSize;

            Point C = new Point(Convert.ToInt32(B.X - cx * 2 + cy), Convert.ToInt32(B.Y - cy * 2 - cx));
            Point D = new Point(Convert.ToInt32(B.X - cx * 2 - cy), Convert.ToInt32(B.Y - cy * 2 + cx));

            g.DrawLine(LinePen, C, B);
            g.DrawLine(LinePen, B, D);
        }

        public void PaintEnd(Graphics g, Point A, Point B)
        {
            double cx = B.X - A.X;
            double cy = B.Y - A.Y;
            double d = Math.Sqrt(cx * cx + cy * cy);
            cx /= d;
            cy /= d;
            cx *= EndingSize;
            cy *= EndingSize;

            Point C = new Point(Convert.ToInt32(B.X + cy), Convert.ToInt32(B.Y - cx));
            Point D = new Point(Convert.ToInt32(B.X - cy), Convert.ToInt32(B.Y + cx));

            g.DrawLine(LinePen, C, D);
        }

    }
}
