using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Xml;

namespace Nyasa_Wizard
{
    [Serializable()]
    public class MNPageMantra: MNPageObject
    {
        private static Pen drawingPen = null;

        private MNPageTextWithImage p_mantra = null;
        private MNPagePoint p_hotspot = null;

        public MNPageTextWithImage Mantra { get { return p_mantra; } }
        public MNPagePoint HotSpot { get { return p_hotspot; } }

        public override List<MNPageObject> Load(XmlElement e)
        {
            List < MNPageObject >  list = base.Load(e);
            foreach (XmlElement e1 in e.ChildNodes)
            {
                if (e1.Name.Equals("MNPagePoint"))
                {
                    HotSpot.Load(e1);
                }
                else if (e1.Name.Equals("MNPageTextWithImage"))
                {
                    Mantra.Load(e1);
                }
            }

            list.Insert(0, HotSpot);
            list.Insert(1, Mantra);
            return list;
        }

        public override XmlElement Save(XmlDocument doc)
        {
            XmlElement e = base.Save(doc);
            e.AppendChild(Mantra.Save(doc));
            e.AppendChild(HotSpot.Save(doc));
            return e;
        }

        public MNPageMantra(MNPage p): base(p)
        {
            p_mantra = new MNPageTextWithImage(p);
            p_mantra.ParentObject = this;

            p_hotspot = new MNPagePoint(p);
            p_hotspot.ParentObject = this;
        }

        public override void Paint(MNPageContext context)
        {
            Rectangle rect = Mantra.GetLogicalRectangle(context);
            Point ch = HotSpot.GetLogicalCenter(context);
            Point cm = new Point();

            if (Mantra.Selected && context.TrackedObjects.Count > 0)
            {
                rect.Offset(context.TrackedDrawOffset);
            }

            if (HotSpot.Selected && context.TrackedObjects.Count > 0)
                ch.Offset(context.TrackedDrawOffset);

            if (ch.X > rect.Left)
                cm = new Point(rect.Left + Mantra.ImageSize.Width + Mantra.TextSize.Width / 2, rect.Top + Mantra.TextSize.Height / 2);
            else
                cm = new Point(rect.Left + Mantra.ImageSize.Width / 2, rect.Top + Mantra.ImageSize.Height / 2);

            context.g.DrawLine(Pens.DarkGray, ch, cm);
            return;

            int dx = cm.X - ch.X;
            int dy = cm.Y - ch.Y;
            int v = Convert.ToInt32(Math.Sqrt(dx * dx + dy * dy));
            int nv = v - HotSpot.Radius;
            double ddx = 0, ddy = 0;
            if (v > 0)
            {
                ddx = (double)dx * (double)nv / v;
                ddy = (double)dy * (double)nv / v;
                ch.X = cm.X - Convert.ToInt32(ddx);
                ch.Y = cm.Y - Convert.ToInt32(ddy);

                ddy = ddy * rect.Width / (double)rect.Height;

                if (drawingPen == null)
                {
                    drawingPen = Pens.DarkGray;
                    //drawingPen = new Pen(Color.Black);
                    //drawingPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                }

                int tmp;
                if (ddx > ddy)
                {
                    if (-ddx > ddy)
                    {
                        // 4
                        //context.g.DrawLine(drawingPen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                        if (ch.X > rect.Right) tmp = rect.Right;
                        else if (ch.X < rect.Left) tmp = rect.Left;
                        else tmp = ch.X;
                        context.g.DrawLine(drawingPen, tmp, rect.Bottom, ch.X, ch.Y);
                    }
                    else
                    {
                        // 1
                        //context.g.DrawLine(drawingPen, rect.Left, rect.Top, rect.Left, rect.Bottom);
                        if (ch.X > rect.Bottom) tmp = rect.Bottom;
                        else if (ch.X < rect.Top) tmp = rect.Top;
                        else tmp = ch.Y;
                        context.g.DrawLine(drawingPen, rect.Left, cm.Y, ch.X, ch.Y);
                    }
                }
                else
                {
                    if (-ddx > ddy)
                    {
                        // 2
                        //context.g.DrawLine(drawingPen, rect.Right, rect.Top, rect.Right, rect.Bottom);
                        if (ch.X > rect.Bottom) tmp = rect.Bottom;
                        else if (ch.X < rect.Top) tmp = rect.Top;
                        else tmp = ch.Y;
                        context.g.DrawLine(drawingPen, rect.Right, tmp, ch.X, ch.Y);
                    }
                    else
                    {
                        // 3
                        //context.g.DrawLine(drawingPen, rect.Left, rect.Top, rect.Right, rect.Top);
                        if (ch.X > rect.Right) tmp = rect.Right;
                        else if (ch.X < rect.Left) tmp = rect.Left;
                        else tmp = ch.X;
                        context.g.DrawLine(drawingPen, tmp, rect.Top, ch.X, ch.Y);
                    }
                }

            }

        }


    }
}
