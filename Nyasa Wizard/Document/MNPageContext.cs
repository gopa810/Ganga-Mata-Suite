using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Nyasa_Wizard
{
    public class MNPageContext
    {
        public Graphics g;
        public int PageWidth;
        public int PageHeight;
        public bool drawSelectionMarks = true;

        public static Image[] HandImages = null;

        public Matrix LastMatrix { get; set; }

        public Matrix LastInvertMatrix { get; set; }

        public List<MNPageObject> TrackedObjects = new List<MNPageObject>();
        public Point TrackedStartLogical = new Point();
        public Point TrackedDrawOffset = new Point();

        public Point PhysicalToRelative(Point clientPoint)
        {
            clientPoint = PhysicalToLogical(clientPoint);
            clientPoint.X = clientPoint.X * 1000 / PageWidth;
            clientPoint.Y = clientPoint.Y * 1000 / PageHeight;
            return clientPoint;
        }

        public Point PhysicalToLogical(Point clientPoint)
        {
            Point[] a = new Point[] { clientPoint };
            LastInvertMatrix.TransformPoints(a);
            return a[0];
        }

        public int PhysicalToLogical(int clientDistance)
        {
            Point[] a = new Point[] { new Point(0, 0), new Point(clientDistance, 0) };
            LastInvertMatrix.TransformPoints(a);
            return a[1].X - a[0].X;
        }


        public Image GetHandImage(int ImageCode)
        {
            if (ImageCode < 0 || ImageCode > 32)
                ImageCode = 0;

            if (HandImages == null)
            {
                HandImages = new Image[33];
                HandImages[0] = new Bitmap(Properties.Resources.h0);
                HandImages[1] = new Bitmap(Properties.Resources.h1);
                HandImages[2] = new Bitmap(Properties.Resources.h2);
                HandImages[3] = new Bitmap(Properties.Resources.h3);
                HandImages[4] = new Bitmap(Properties.Resources.h4);
                HandImages[5] = new Bitmap(Properties.Resources.h5);
                HandImages[6] = new Bitmap(Properties.Resources.h6);
                HandImages[7] = new Bitmap(Properties.Resources.h7);
                HandImages[8] = new Bitmap(Properties.Resources.h8);
                HandImages[9] = new Bitmap(Properties.Resources.h9);
                HandImages[10] = new Bitmap(Properties.Resources.h10);
                HandImages[11] = new Bitmap(Properties.Resources.h11);
                HandImages[12] = new Bitmap(Properties.Resources.h12);
                HandImages[13] = new Bitmap(Properties.Resources.h13);
                HandImages[14] = new Bitmap(Properties.Resources.h14);
                HandImages[15] = new Bitmap(Properties.Resources.h15);
                HandImages[16] = new Bitmap(Properties.Resources.h16);
                HandImages[17] = new Bitmap(Properties.Resources.h17);
                HandImages[18] = new Bitmap(Properties.Resources.h18);
                HandImages[19] = new Bitmap(Properties.Resources.h19);
                HandImages[20] = new Bitmap(Properties.Resources.h20);
                HandImages[21] = new Bitmap(Properties.Resources.h21);
                HandImages[22] = new Bitmap(Properties.Resources.h22);
                HandImages[23] = new Bitmap(Properties.Resources.h23);
                HandImages[24] = new Bitmap(Properties.Resources.h24);
                HandImages[25] = new Bitmap(Properties.Resources.h25);
                HandImages[26] = new Bitmap(Properties.Resources.h26);
                HandImages[27] = new Bitmap(Properties.Resources.h27);
                HandImages[28] = new Bitmap(Properties.Resources.h28);
                HandImages[29] = new Bitmap(Properties.Resources.h29);
                HandImages[30] = new Bitmap(Properties.Resources.h30);
                HandImages[31] = new Bitmap(Properties.Resources.h31);
                HandImages[32] = new Bitmap(Properties.Resources.h32);
            }

            return HandImages[ImageCode];
        }
    }
}
