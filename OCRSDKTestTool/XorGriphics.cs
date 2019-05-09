using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{
    public class XorGraphics
    {
        #region Enumerations
        public enum DrawingMode : int
        {
            R2_BLACK = 1,
            R2_NOTMERGEPEN,
            R2_MASKNOTPEN,
            R2_NOTCOPYPEN,
            R2_MASKPENNOT,
            R2_NOT,
            R2_XORPEN,
            R2_NOTMASKPEN,
            R2_MASKPEN,
            R2_NOTXORPEN,
            R2_NOP,
            R2_MERGENOTPEN,
            R2_COPYPEN,
            R2_MERGEPENNOT,
            R2_MERGEPEN,
            R2_WHITE,
            R2_LAST
        }

        public enum BrushStyles:int
        {
            BS_SOLID = 0,
            BS_NULL = 1,
            BS_HATCHED = 2,
            BS_PATTERN = 3,
            BS_INDEXED = 4,
            BS_DIBPATTERN = 5,
            BS_DIBPATTERNPT = 6,
            BS_PATTERN8X8 = 7,
            BS_MONOPATTERN = 9
        }


        public enum PenStyles : int
        {
            PS_SOLID = 0,
            PS_DASH = 1,
            PS_DOT = 2,
            PS_DASHDOT = 3,
            PS_DASHDOTDOT = 4
        }
        #endregion
        [DllImport("Gdi32.dll")]
        private static extern IntPtr CreatePen(int fnPenStyle, int width, int color);

        [DllImport("Gdi32.dll")]
        private static extern int SetROP2(System.IntPtr hdc, int rop);

        [DllImport("Gdi32.dll")]
        private static extern int MoveToEx(IntPtr hdc, int x, int y, IntPtr lppoint);

        [DllImport("Gdi32.dll")]
        private static extern int LineTo(IntPtr hdc, int X, int Y);

        [DllImportAttribute("gdi32.dll")]
        private static extern bool Rectangle(IntPtr hdc, int X1, int Y1, int X2, int Y2);

        [DllImport("Gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObj);

        private Graphics g;

        public XorGraphics(Graphics g)
        {
            this.g = g;
        }

        private int ArgbToRGB(int rgb)
        {
            return ((rgb >> 16 & 0x0000FF) | (rgb & 0x00FF00) | (rgb << 16 & 0xFF0000));
        }

        public void DrawRectangle(Pen pen, Rectangle rectangle)
        {
            IntPtr hDC = g.GetHdc();
            IntPtr hPen = CreatePen((int)PenStyles.PS_DASH, (int)pen.Width, ArgbToRGB(pen.Color.ToArgb()));
            SelectObject(hDC, hPen);
            SetROP2(hDC, (int)DrawingMode.R2_NOTXORPEN);
            Rectangle(hDC, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
            DeleteObject(hPen);
            g.ReleaseHdc(hDC);
        }
    }
}
