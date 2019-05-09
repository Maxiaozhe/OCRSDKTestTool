using DoOcrSDKInterface;
using HocrSDKInterface;
using JocrSDKInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OCRSDKTest
{
    public class SDKArea
    {
        public int xe;
        public int xs;
        public int ye;
        public int ys;

        public Rectangle ToRect()
        {
            int x = xs<xe?xs:xe;
            int y=ys<ye?ys:ye;
            int width = Math.Abs( xe-xs);
            int height =Math.Abs( ye-ys);
            return new Rectangle(x,y,width,height);
        }

        public SDKArea(HocrSDKArea area)
        {
            this.xs = area.xs;
            this.xe = area.xe;
            this.ys = area.ys;
            this.ye = area.ye;
        }

        public SDKArea(JocrSDKArea area)
        {
            this.xs = area.xs;
            this.xe = area.xe;
            this.ys = area.ys;
            this.ye = area.ye;
        }

        public SDKArea(DoOcrSDKArea area)
        {
            this.xs = area.xs;
            this.xe = area.xe;
            this.ys = area.ys;
            this.ye = area.ye;
        }
    }
}
