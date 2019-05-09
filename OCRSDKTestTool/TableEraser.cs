using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{

    public enum LineDirction
    {
        Horizontal,
        Vertical,
    }
    /// <summary>
    /// 罫線処理のパラメタ変数設定
    /// </summary>
    public class EraserParams
    {
        /// </summary>
        private const int DEF_LINERECT_RATIO = 12;
        private const int DEF_MIN_LENGTH = 64;
        private const int DEF_MIN_SCAN_LENGTH = 8;
        private const int DEF_MAX_DOT_SPACE = 4;
        private const int DEF_H_STEP = 8;
        private const int DEF_BORDER_REGION = 16;
        private const int DEF_EXTRACTFRAME_MARGIN = 20;

        /// <summary>
        /// 罫線矩形の縦横比のしきい値
        /// </summary>
        public int LineRectRatio { get; set; }
        /// <summary>
        /// 罫線矩形の長さの最小しきい値
        /// </summary>
        public int MinLenght { get; set; }
        /// <summary>
        /// 罫線走査の最小しきい値
        /// </summary>
        public int MinScanLength { get; set; }
        /// <summary>
        /// 点線の最大間隔値
        /// </summary>
        public int MaxDotSpace { get; set; }
        /// <summary>
        /// 高速走査ステップ
        /// </summary>
        public int HighSpeedStep { get; set; }

        public int BorderRegion { get; set; }

        /// <summary>
        /// 表領域からセル詳細データを抽出する時、
        /// 枠領域IDを決定する際の誤差修正しきい値
        /// </summary>
        public int ExtractFrameMargin { get; set; }

        public EraserParams()
        {
            this.LineRectRatio = DEF_LINERECT_RATIO;
            this.MinLenght = DEF_MIN_LENGTH;
            this.MinScanLength = DEF_MIN_SCAN_LENGTH;
            this.MaxDotSpace = DEF_MAX_DOT_SPACE;
            this.HighSpeedStep = DEF_H_STEP;
            this.BorderRegion = DEF_BORDER_REGION;
            this.ExtractFrameMargin = DEF_EXTRACTFRAME_MARGIN;
        }

    }
    /// <summary>
    /// 罫線処理のクラスを実装
    /// </summary>
    public class TableEraser
    {
        private static EraserParams _env = null;

        public static EraserParams Env
        {
            get
            {
                if (_env == null)
                {
                    _env = new EraserParams();
                    return _env;
                }
                return _env;

            }
        }
        /// <summary>
        /// パラメタを設定する
        /// </summary>
        /// <param name="minLength">罫線矩形の長さの最小しきい値</param>
        /// <param name="lineRectRatio">罫線矩形の縦横比のしきい値</param>
        /// <param name="minScanLength">罫線走査の最小しきい値</param>
        /// <param name="maxDotSpace">点線の最大間隔値</param>
        public static void SetParams(EraserParams env)
        {
            _env = env;
        }

        public TableEraser()
        {

        }

        private Counter.SubOutputHandler outputHanler = null;
        public TableEraser(Counter.SubOutputHandler handler)
        {
            outputHanler = handler;
        }

        public Image EraseLines(Bitmap targetImg, Color color)
        {
            Counter counter = new Counter(this.outputHanler);
            counter.Start("EraseLines");
            //水平線
            Bitmap newImg = targetImg.Clone(new Rectangle(0, 0, targetImg.Width, targetImg.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            LockBitmap imgLocker = new LockBitmap(newImg);
            imgLocker.LockBits();
            //TestGetPixle(imgLocker);
            List<MergeLineInfo> Hlines = ScanLines(imgLocker, LineDirction.Horizontal);
            //縦線
            List<MergeLineInfo> Vlines = ScanLines(imgLocker, LineDirction.Vertical);
            EraseAllLines(color, imgLocker, Hlines, Vlines);
            imgLocker.UnlockBits();
            counter.End();
            return newImg;
        }

        public Image EraseLines(Bitmap targetImg, Color color, List<Rectangle> Regions)
        {
            if (Regions.Count == 0)
            {
                return EraseLines(targetImg, color);
            }
            Counter counter = new Counter(this.outputHanler);
            counter.Start("EraseLines");
            //水平線
            Bitmap newImg = targetImg.Clone(new Rectangle(0, 0, targetImg.Width, targetImg.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            LockBitmap imgLocker = new LockBitmap(newImg);
            imgLocker.LockBits();
            //TestGetPixle(imgLocker);
            List<MergeLineInfo> Hlines = ScanLines(imgLocker, LineDirction.Horizontal);
            //縦線
            List<MergeLineInfo> Vlines = ScanLines(imgLocker, LineDirction.Vertical);
            imgLocker.UnlockBits();
            using (Graphics g = Graphics.FromImage(newImg))
            {
                EraseAllLines(g, Hlines, Vlines, Regions);
            }
            counter.End();
            return newImg;
        }


        private void EraseAllLines(Color color, LockBitmap imgLocker, List<MergeLineInfo> Hlines, List<MergeLineInfo> Vlines)
        {
            Counter counter = new Counter(this.outputHanler);
            counter.Start("EraseHorizontalLines");
            //水平線クリア
            Hlines.ForEach(l => l.Erase(imgLocker, color));
            counter.End();
            //縦線クリア
            counter.Start("EraseVerticalLines");
            Vlines.ForEach(l => l.Erase(imgLocker, color));
            counter.End();
        }

        private void EraseAllLines(Graphics g, List<MergeLineInfo> Hlines, List<MergeLineInfo> Vlines, List<Rectangle> Regions)
        {
            Counter counter = new Counter(this.outputHanler);
            counter.Start("EraseHorizontalLines");
            //水平線クリア
            Hlines.ForEach(l => EraseLine(g,l,Regions ));
            counter.End();
            //縦線クリア
            counter.Start("EraseVerticalLines");
            Vlines.ForEach(l => EraseLine(g, l, Regions));
            counter.End();
        }

        private void EraseLine(Graphics g,MergeLineInfo mline, List<Rectangle> Regions)
        {
            foreach (Rectangle reg in Regions)
            {
                Rectangle? nearRect = GetNearRegion(mline, reg);
                if (nearRect.HasValue)
                {
                    g.FillRectangle(Brushes.White, nearRect.Value);
                }
            }
        }

        private Rectangle? GetNearRegion(MergeLineInfo mline, Rectangle rect)
        {
            Rectangle border1;
            Rectangle border2;
            Rectangle mlineRect = mline.GetRect();
            if (!mline.IsLine) return null;
            if (mline.LineType == LineDirction.Horizontal)
            {
                border1 = new Rectangle(rect.X - Env.BorderRegion, rect.Y - Env.BorderRegion, rect.Width + Env.BorderRegion * 2, Env.BorderRegion * 2);
                border2 = new Rectangle(rect.X - Env.BorderRegion, rect.Y + rect.Height - Env.BorderRegion, rect.Width + Env.BorderRegion * 2, Env.BorderRegion * 2);
            }
            else
            {
                border1 = new Rectangle(rect.X - Env.BorderRegion, rect.Y - Env.BorderRegion, Env.BorderRegion * 2, rect.Height + Env.BorderRegion*2);
                border2 = new Rectangle(rect.X + rect.Width - Env.BorderRegion, rect.Y - Env.BorderRegion, Env.BorderRegion * 2, rect.Height + Env.BorderRegion*2);
            }
            if (mlineRect.IntersectsWith(border1))
            {
                mlineRect.Intersect(border1);
                return mlineRect;
            }
            else if (mlineRect.IntersectsWith(border2))
            {
                mlineRect.Intersect(border2);
                return mlineRect;
            }
            if (rect.IntersectsWith(mlineRect))
            {
                if (mline.LineType == LineDirction.Horizontal && mlineRect.Width>=rect.Width)
                {
                    mlineRect.Intersect(rect);
                    return mlineRect;
                }
                else if (mline.LineType == LineDirction.Vertical && mlineRect.Height >= rect.Height)
                {
                    mlineRect.Intersect(rect);
                    return mlineRect;
                }

            }
            return null;
        }



        public Image DrawLineRects(Bitmap orgImg, Color color)
        {
            //水平線
            Bitmap newImg = orgImg.Clone(new Rectangle(0, 0, orgImg.Width, orgImg.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            LockBitmap imgLocker = new LockBitmap(newImg);
            imgLocker.LockBits();
            List<MergeLineInfo> Hlines = ScanLines(imgLocker, LineDirction.Horizontal);
            //縦線
            List<MergeLineInfo> Vlines = ScanLines(imgLocker, LineDirction.Vertical);
            imgLocker.UnlockBits();

            Bitmap drawImg = new Bitmap(newImg);
            //水平線描画
            Hlines.ForEach(l => l.DrawRect(drawImg));
            //縦線描画
            Vlines.ForEach(l => l.DrawRect(drawImg));
            return drawImg;
        }



        private List<MergeLineInfo> ScanLines(LockBitmap img, LineDirction lineType)
        {
            Counter counter = new Counter(outputHanler);
            counter.Start("ScanLines -" + lineType.ToString());
            LineInfo line = null;
            List<LineInfo> lines = new List<LineInfo>();
            int maxStep = lineType == LineDirction.Horizontal ? img.Height : img.Width;
            int maxLength = lineType == LineDirction.Horizontal ? img.Width : img.Height;
            int ystep = 1;
            int xstep = 1;
            int bigStep = Env.HighSpeedStep;
            for (int y = 0; y < maxStep; y += ystep)
            {
                List<LineInfo> currentLines = new List<LineInfo>();
                line = new LineInfo(y, lineType);
                for (int x = 0; x < maxLength; x += xstep)
                {
                    Color color = lineType == LineDirction.Horizontal ? img.GetPixel(x, y) : img.GetPixel(y, x);
                    if (color.ToArgb() != Color.Black.ToArgb())
                    {
                        if (line.Start > -1)
                        {
                            currentLines.Add(line);
                            line = new LineInfo(y, lineType);
                        }
                        xstep = bigStep;
                    }
                    else
                    {
                        if (xstep == bigStep && x > bigStep - 1)
                        {
                            x = SearchBlackStart(img, x - bigStep + 1, y, maxLength, lineType);
                        }
                        xstep = 1;
                        if (line.Start == -1)
                        {
                            line.Start = x;
                            line.End = x;
                        }
                        else
                        {
                            line.End = x;
                        }

                    }
                }
                if (line.Start != -1)
                {
                    currentLines.Add(line);
                }
                if (currentLines.Count > 0)
                {
                    List<LineInfo> dotLines = MergeDotLine(currentLines, Env.MaxDotSpace);
                    foreach (LineInfo dotLine in dotLines)
                    {
                        if (dotLine.isLine())
                        {
                            lines.Add(dotLine);
                        }
                    }
                }
                currentLines = null;
            }
            counter.End();
            return GetMergeLines(lines);
        }

        private int SearchBlackStart(LockBitmap img, int x, int y, int maxLength, LineDirction lineType)
        {
            Color color = lineType == LineDirction.Horizontal ? img.GetPixel(x, y) : img.GetPixel(y, x);
            while (color.ToArgb() != Color.Black.ToArgb() && x < maxLength)
            {
                x++;
                color = lineType == LineDirction.Horizontal ? img.GetPixel(x, y) : img.GetPixel(y, x);
            }
            return x;
        }

        private void TestGetPixle(LockBitmap img)
        {
            Counter counter = new Counter(outputHanler);
            counter.Start("TestGetPixle");
            int maxStep = img.Height;
            int maxLength = img.Width;
            for (int y = 0; y < maxStep; y++)
            {
                for (int x = 0; x < maxLength; x++)
                {
                    Color color = img.GetPixel(x, y);
                }
            }
            counter.End();
        }

        private List<LineInfo> MergeDotLine(List<LineInfo> lines, int dotSpace)
        {
            List<LineInfo> resultLines = new List<LineInfo>();
            if (lines == null || lines.Count == 0)
            {
                return resultLines;
            }
            LineInfo currentLine = null;
            foreach (LineInfo line in lines)
            {
                if (currentLine == null)
                {
                    currentLine = line;
                    continue;
                }
                if (currentLine.End + dotSpace >= line.Start)
                {
                    currentLine.End = line.End;
                }
                else
                {
                    resultLines.Add(currentLine);
                    currentLine = line;
                }
            }
            resultLines.Add(currentLine);
            return resultLines;
        }

        private List<MergeLineInfo> GetMergeLines(List<LineInfo> lines)
        {
            Counter counter = new Counter(outputHanler);
            counter.Start("GetMergeLines");
            List<MergeLineInfo> mlines = new List<MergeLineInfo>();
            foreach (LineInfo line in lines)
            {
                MergeLineInfo mline = mlines.Find(x => x.CanMerge(line));
                if (mline != null)
                {
                    mline.Merge(line);
                }
                else
                {
                    mlines.Add(new MergeLineInfo(line));
                }
            }
            counter.End();
            return mlines;
        }
    }

    /// <summary>
    /// 罫線情報
    /// </summary>
    public class LineInfo
    {
        /// <summary>
        /// 罫線長さのしきい値
        /// </summary>
        private int MinScanLength = 8;

        public int Pos { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public LineDirction LineType { get; set; }
        private Rectangle rect;
        public bool isLine()
        {
            return (End - Start >= this.MinScanLength);
        }
        public LineInfo(int pos, LineDirction lineType)
        {
            this.LineType = lineType;
            this.Pos = pos;
            this.Start = -1;
            this.End = -1;
            this.MinScanLength = TableEraser.Env.MinScanLength;
        }

        public Rectangle LineRect
        {
            get
            {
                if (rect.IsEmpty)
                {
                    rect = new Rectangle(Start, Pos, End - Start, 2);
                }
                return rect;
            }
        }

        public bool ConnectWith(LineInfo line)
        {
            if (Math.Abs(line.Pos - this.Pos) > 1)
            {
                return false;
            }
            if (this.End + 1 > line.Start && this.Start <= line.End + 1)
            {
                //if ((this.Start - MIN_LENGTH > line.Start && this.End + MIN_LENGTH < line.End)
                //    || (line.Start - MIN_LENGTH > this.Start && line.End + MIN_LENGTH < this.End))
                //{
                //    return false;
                //}
                return true;
            }
            return false;
        }


        public void Erase(LockBitmap img, Color color)
        {
            for (int x = Start; x <= End; x++)
            {
                if (LineType == LineDirction.Horizontal)
                {
                    img.SetPixel(x, this.Pos, color);
                }
                else
                {
                    img.SetPixel(this.Pos, x, color);
                }
            }
        }
    }
    /// <summary>
    /// マッジする罫線矩形情報
    /// </summary>
    public class MergeLineInfo
    {
        /// <summary>
        ///  罫線矩形の縦横比のしきい値
        /// </summary>
        //private const int LINERECT_RATIO = 12;
        //private const int MIN_LENGTH = 64;



        public List<LineInfo> Lines { get; set; }
        public LineDirction LineType { get; set; }
        private int start;
        private int end;
        private int minPos;
        private int maxPos;

        private void SetInfo()
        {
            if (Lines.Count == 0) return;
            this.start = Lines.Min(x => x.Start);
            this.end = Lines.Max(x => x.End);
            this.minPos = Lines.Min(x => x.Pos);
            this.maxPos = Lines.Max(x => x.Pos);
        }

        public int Length
        {
            get
            {
                return this.end - this.start;
            }
        }
        public int LineWidth
        {
            get
            {
                return this.maxPos - this.minPos + 1;
            }
        }
        public bool IsLine
        {
            get
            {
                return this.Length >= TableEraser.Env.MinLenght &&
                    (this.Length / this.LineWidth) > TableEraser.Env.LineRectRatio;
            }
        }

        public MergeLineInfo(LineInfo line)
        {
            this.LineType = line.LineType;
            this.Lines = new List<LineInfo>();
            this.Lines.Add(line);
            SetInfo();
        }

        public bool CanMerge(LineInfo line)
        {
            if (this.Lines.Count == 0)
            {
                return true;
            }
            if (line.Pos > this.maxPos + 1 || line.Pos < this.minPos - 1)
            {
                return false;
            }
            bool CanMerge = this.Lines.Exists(x =>
            {
                //  return (x.LineType == line.LineType) && Math.Abs(x.Pos - line.Pos) <= 1 && x.LineRect.IntersectsWith(line.LineRect);
                return (x.LineType == line.LineType) && x.ConnectWith(line);
            });
            return CanMerge;
        }

        public bool Merge(LineInfo line)
        {
            if (this.CanMerge(line))
            {
                this.Lines.Add(line);
                SetInfo();
                return true;
            }
            return false;
        }


        public void Erase(LockBitmap img, Color color)
        {
            if (this.IsLine)
            {
                foreach (LineInfo line in this.Lines)
                {
                    line.Erase(img, color);
                }
            }
        }
        public void Erase(Graphics g, Color color)
        {
            if (this.IsLine)
            {
                g.FillRectangle(new SolidBrush(color), this.GetRect());
            }
        }



        public Rectangle GetRect()
        {
            if (this.LineType == LineDirction.Horizontal)
            {
                int left = this.start;
                int top = this.minPos;
                return new Rectangle(left, top, this.Length, this.LineWidth);
            }
            else
            {
                int left = this.minPos;
                int top = this.start;
                return new Rectangle(left, top, this.LineWidth, this.Length);
            }
        }

        public void DrawRect(Bitmap img)
        {
            if (this.IsLine)
            {
                Rectangle rect;
                if (this.LineType == LineDirction.Horizontal)
                {
                    int left = Lines.Min(x => x.Start);
                    int top = Lines.Min(x => x.Pos);
                    rect = new Rectangle(left, top, this.Length, this.LineWidth);
                }
                else
                {
                    int left = Lines.Min(x => x.Pos);
                    int top = Lines.Min(x => x.Start);
                    rect = new Rectangle(left, top, this.LineWidth, this.Length);
                }
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.DrawRectangle(Pens.Red, rect);
                }
            }
        }

    }


}
