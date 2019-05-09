using DoOcrSDKInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{
    public class BarcodeFrame:Frame
    {
        public Bitmap BarcodeImage { get; set; }
        public BarcodeFrame()
        {
            
        }
        public BarcodeFrame(Rectangle rect):base(rect)
        {
           
        }

        public void Draw(ZoomedGraphics g)
        {
            if (this.BarcodeImage == null)
            {
                g.DrawRectangle(Pens.Red, this.Rect);
            }
            else
            {
                g.DrawImage(this.BarcodeImage, this.StartPoint);
                if (this.IsSelected)
                {
                    g.DrawRectangle(Pens.Red, this.Rect);
                }
            }
        }

        public string RawText { get; set; }

        public ZXing.BarcodeFormat BarcodeFormat { get; set; }

        public bool CanCreate { get; set; }
    }

    public class Frame
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public bool IsSelected { get; set; }
        public bool IsEmpty
        {
            get
            {
                return StartPoint.Equals(EndPoint);
            }
        }
        public bool IsTooSmall
        {
            get
            {
                return this.Rect.Height <= 8 || this.Rect.Width <= 8;
            }
        }
        public Rectangle Rect
        {
            get
            {
                int x = Math.Min(StartPoint.X, EndPoint.X);
                int y = Math.Min(StartPoint.Y, EndPoint.Y);
                int w = Math.Abs(EndPoint.X - StartPoint.X);
                int h = Math.Abs(EndPoint.Y - StartPoint.Y);
                return new Rectangle(x, y, w, h);
            }
            set
            {
                this.StartPoint = new Point(value.X, value.Y);
                this.EndPoint = new Point(value.X + value.Width, value.Y + value.Height);
            }
        }
        public Frame()
        {

        }
        public Frame(Point Start)
        {
            this.StartPoint = Start;
            this.EndPoint = Start;
            this.IsSelected = false;
        }

        public Frame(Rectangle rect)
        {
            this.StartPoint = new Point(rect.X, rect.Y);
            this.EndPoint = new Point(rect.X + rect.Width, rect.Y + rect.Height);
            this.IsSelected = false;
        }
        public override bool Equals(object obj)
        {
            if (obj is Frame)
            {
                return this.Rect.Equals(((Frame)obj).Rect);
            }
            if (obj is Rectangle)
            {
                return this.Rect.Equals(obj);
            }
            return base.Equals(obj);
        }



        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

    public class FrameCollection<T> 
        where T:Frame,new()
    {
        private List<T> _frames = new List<T>();

        public FrameCollection()
        {

        }

        public FrameCollection(IList<Rectangle> rects)
        {
            AddRange(rects);
        }

        public List<T> Items
        {
            get
            {
                return this._frames;
            }
        }

        public void Add(Rectangle rect)
        {
            if (!this._frames.Exists(x => x.Equals(rect)))
            {
                this._frames.Add(new T() { Rect=rect });
            }
        }

        public void Add(T frm)
        {
            if (!this._frames.Exists(x => x.Equals(frm)))
            {
                this._frames.Add(frm);
            }
        }

        public void AddRange(IList<Rectangle> rects)
        {
            foreach (Rectangle rect in rects)
            {
                if (!this._frames.Exists(x => x.Equals(rect)))
                {
                    this._frames.Add(new T() { Rect = rect });
                }
            }
        }

        public void AddRange(IList<T> frames)
        {
            foreach (T frm in frames)
            {
                if (!this._frames.Exists(x => x.Equals(frm)))
                {
                    this._frames.Add(frm);
                }
            }
        }

        public T GetFrameByPoint(int x, int y)
        {
            Point pt = new Point(x, y);
            if (this._frames.Count == 0)
            {
                return null;
            }
            var frms = this._frames.FindAll(frm => frm.Rect.Contains(pt));
            if (frms.Count == 0)
            {
                return null;
            }
            return frms.Last();
        }

        public T GetFrame(Rectangle rect)
        {
            if (this._frames.Count == 0)
            {
                return null;
            }
            return this._frames.Find(x => x.Equals(rect));
        }

        public T GetFrame(T frm)
        {
            if (this._frames.Count == 0)
            {
                return null;
            }
            return this._frames.Find(x => x.Equals(frm));
        }

        public void Clear()
        {
            this._frames.Clear();
        }

        public void ClearSelection()
        {
            var frms = this._frames.Where(x => x.IsSelected);
            if (frms.Count() > 0)
            {
                foreach (T frm in frms)
                {
                    frm.IsSelected = false;
                }
            }
        }

        public List<Rectangle> ToAllRects()
        {
            var Rects = this._frames.Select(x => x.Rect);
            return Rects.ToList();
        }

        public List<Rectangle> ToNormalRects()
        {
            var Rects = this._frames.Where(x => !x.IsSelected).Select(x => x.Rect);
            return Rects.ToList();
        }
        public List<Rectangle> ToSelectedRects()
        {
            var Rects = this._frames.Where(x => x.IsSelected).Select(x => x.Rect);
            return Rects.ToList();
        }

        public void Draw(ZoomedGraphics zg)
        {
            List<Rectangle> normalRects = this.ToNormalRects();
            if (normalRects.Count > 0)
            {
                zg.DrawRectangles(Pens.Blue, normalRects.ToArray());
            }
            List<Rectangle> selRets = this.ToSelectedRects();
            if (selRets.Count > 0)
            {
                zg.DrawRectangles(Pens.Red, this.ToSelectedRects().ToArray());
            }
        }

    }

    public class TransArea
    {
        public int status { get; set; }
        public Rectangle TransRect { get; set; }
        public Rectangle CharRect { get; set; }

        /// <summary>
        /// 文字認識領域を取得する
        /// </summary>
        /// <returns></returns>
        public DoOcrSDKArea GetCharArea()
        {
            DoOcrSDKArea area;
            if (this.CharRect.IsEmpty || !this.TransRect.Contains(this.CharRect))
            {
                //文字領域が設定されないあるいは文字領域は不正の場合、項目領域を使用する
                //(項目画像のため画像全体だが、ピッタリだとDoOCRがうまく動かないため)
                area = new DoOcrSDKArea()
                {
                    xs = (short)1,
                    ys = (short)1,
                    xe = (short)(TransRect.Width - 1),
                    ye = (short)(TransRect.Height - 1),
                };
            }
            else
            {
                area = new DoOcrSDKArea()
                {
                    xs = (short)(CharRect.X == TransRect.X ? CharRect.X - TransRect.X + 1 : CharRect.X - TransRect.X),
                    ys = (short)(CharRect.Y == TransRect.Y ? CharRect.Y - TransRect.Y + 1 : CharRect.Y - TransRect.Y),
                    xe = (short)(CharRect.Width == TransRect.Width ? CharRect.X - TransRect.X + CharRect.Width - 1 : CharRect.X - TransRect.X + CharRect.Width),
                    ye = (short)(CharRect.Height == TransRect.Height ? CharRect.Y - TransRect.Y + CharRect.Height - 1 : CharRect.Y - TransRect.Y + CharRect.Height),
                };
            }
            return area;
        }


    }

}
