using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace Nyasa_Wizard
{
    [Serializable()]
    public class MNPagePoint : MNPageObject
    {
        public int Radius { get; set; }

        public bool EditParentProperties { get; set; }

        public MNPagePoint(MNPage p): base(p)
        {
            EditParentProperties = false;
        }

        public override XmlElement Save(XmlDocument doc)
        {
            XmlElement e = base.Save(doc);
            e.SetAttribute("radius", Radius.ToString());
            e.SetAttribute("EditParentProperties", EditParentProperties.ToString());
            return e;
        }

        public override List<MNPageObject> Load(XmlElement e)
        {
            List < MNPageObject > list =  base.Load(e);

            Radius = int.Parse(e.GetAttribute("radius"));
            EditParentProperties = bool.Parse(e.GetAttribute("EditParentProperties"));

            return list;
        }

        public override MNPageObject TestHitLogical(MNPageContext context, Point logicalPoint)
        {
            Point logicalCenter = GetLogicalCenter(context);

            int dist = Math.Max(Math.Abs(logicalPoint.X - logicalCenter.X),
                Math.Abs(logicalPoint.Y - logicalCenter.Y));

            return (dist <= Math.Max(40, Radius) ? this : null);
        }

        /// <summary>
        /// returns logical coordinates of point, 
        /// it takes into account whether point is linked to page or image
        /// </summary>
        /// <returns></returns>
        public Point GetLogicalCenter(MNPageContext context)
        {
            Point logicalCenter = new Point();

            logicalCenter.X = RelativeCenter.X * Document.PageWidth / 1000;
            logicalCenter.Y = RelativeCenter.Y * Document.PageHeight / 1000;

            return logicalCenter;
        }

        public override void Paint(MNPageContext context)
        {
            Point lc = GetLogicalCenter(context);
            int radius = Radius;

            if (Selected && context.TrackedObjects.Count > 0)
            {
                lc.Offset(context.TrackedDrawOffset);
            }

            if (Radius > 0)
                context.g.FillEllipse(Brushes.OrangeRed, lc.X - radius, lc.Y - radius, radius * 2, radius * 2);

            if (Selected && context.drawSelectionMarks)
            {
                int rad = Math.Max(40, radius);
                PaintSelectionMarks(context, lc.X - rad, lc.Y - rad, lc.X + rad, lc.Y + rad);
            }
        }
    }


}
