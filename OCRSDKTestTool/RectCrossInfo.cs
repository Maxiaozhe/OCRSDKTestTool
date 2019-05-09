using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{
    public class RectCrossInfo
    {
        public List<Rectangle> Rects;
        public List<Point> Crosses;
        public RectCrossInfo()
        {
            this.Rects = new List<Rectangle>();
            this.Crosses = new List<Point>();
        }
        internal RectCrossInfo( FormSDKInterface.FormSDKRectCrossInfo crossInfo)
        {
            this.Rects = new List<Rectangle>();
            this.Crosses = new List<Point>();
            if (crossInfo == null) return;
            if (crossInfo.rect != null && crossInfo.rect.Count > 0)
            {
                var rects = crossInfo.rect.Select(x =>
                {
                    var area = x.area;
                    return new Rectangle()
                    {
                        X = Math.Min(area.xs, area.xe),
                        Y = Math.Min(area.ys, area.ye),
                        Width = Math.Abs(area.xe - area.xs),
                        Height = Math.Abs(area.ye - area.ys)
                    };
                });
                this.Rects.AddRange(rects.ToArray());
            }
            if (crossInfo.cross != null && crossInfo.cross.Count > 0)
            {
                var points = crossInfo.cross.Select(p => new Point(p.x, p.y));
                this.Crosses.AddRange(points.ToArray());
            }
        }
    }
}
