using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{
    public class ZoomedGraphics
    {
        private Graphics g;
        private float zoom;
        public ZoomedGraphics(Graphics graphics,float zoom)
        {
            this.g = graphics;
            
            this.zoom = zoom;
            if (this.zoom <= 0)
            {
                this.zoom = 1.0F;
            }
        }


        private RectangleF ZoomedRect(Rectangle rect)
        {
            return new RectangleF(rect.X * this.zoom, rect.Y * this.zoom, rect.Width * this.zoom, rect.Height * zoom);
        }
        private PointF ZoomedPoint(Point pt)
        {
            return new PointF(pt.X * this.zoom, pt.Y * this.zoom);
        }

        private RectangleF[] ZoomedRects(Rectangle[] rects)
        {
            return rects.Select(x => ZoomedRect(x)).ToArray();
        }


        public void DrawRectangle(Pen pen, Rectangle rects)
        {
            RectangleF rectf = ZoomedRect(rects);
            g.DrawRectangle(pen, Rectangle.Truncate( rectf));
        }

        public void DrawRectangles( Pen pen, Rectangle[] rects)
        {
            RectangleF[] rectfs = ZoomedRects(rects);
            g.DrawRectangles(pen, rectfs);
        }

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
        {
            g.DrawLine(pen, x1 * this.zoom, y1 * this.zoom, x2 * zoom, y2 * zoom);
        }

        public void DrawImage(Bitmap bitmap, Point point)
        {
            PointF pt = ZoomedPoint(point);
            RectangleF rect=ZoomedRect(new Rectangle(0,0,bitmap.Width,bitmap.Height));
            Bitmap drawImg = new Bitmap(bitmap, (int)rect.Width, (int)rect.Height); 
            g.DrawImage(drawImg, pt);
            
        }
    }
}
