using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCRSDKTest
{
    public class GridPicture : CanvasPicture
    {

        public enum GridMode
        {
            DottedGrid,
            DotLineGrid,
            Grid

        }

        public GridMode GridKind
        {
            get;set;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            DrawGridByBrush(pevent.Graphics);
        }

        private void DrawGridByBrush(Graphics g)
        {
            Rectangle area = this.ClientRectangle;
            Size gridsize = new System.Drawing.Size(10, 10);
            switch (this.GridKind)
            {
                case GridMode.DottedGrid:
                    ControlPaint.DrawGrid(g, area, gridsize, Color.LightGray);
                    break;
                case GridMode.DotLineGrid:
                case GridMode.Grid:
                    HatchStyle bstyle = this.GridKind == GridMode.DotLineGrid ? HatchStyle.DottedGrid : HatchStyle.LargeGrid;
                    using (HatchBrush hbrush = new HatchBrush(bstyle, Color.Gray, Color.White))
                    {
                        g.FillRectangle(hbrush, this.ClientRectangle);
                    }
                    break;
                default:
                    break;
            }

 
            
        }
    }
}
