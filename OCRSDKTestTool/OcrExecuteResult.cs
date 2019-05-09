using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OCRSDKTest
{
    /// <summary>
    /// 帳票認識：OCR結果エンティティ
    /// </summary>
    public class OcrExecuteResult
    {
        /// <summary>
        /// 手書きOCR結果
        /// </summary>
        public List<SDKResult> HocrResultList { get; set; }

        // <summary>
        /// 活字OCR結果
        /// </summary>
        public List<SDKResult> JocrResultList { get; set; }

        /// <summary>
        /// 活字、手書き結果利用判定
        /// </summary>
        public RcFontType ResultType { get; set; }

        /// <summary>
        /// OCR結果文字列
        /// </summary>
        public string ResultCharLine { get; set; }

        public OcrExecuteResult()
        {
            HocrResultList = new List<SDKResult>();
            JocrResultList = new List<SDKResult>();
        }

        private double GetAvgCertainty(List<SDKResult> resultList)
        {
            var targetResult = resultList.Where(x => !(x.cand[0].code[0] == 0x0a && x.cand[0].code[1] == 0x00));
            if (targetResult.Count() > 0)
            {
                return targetResult.Average(x => x.certainty);
            }
            return 0;
        }
        private string GetString(List<SDKResult> resultList)
        {
            List<byte> bytes = new List<byte>();
            foreach (SDKResult data in resultList)
            {
                if (data.cand[0].code[0] == 0x0a && data.cand[0].code[1] == 0x00)
                {
                    continue;
                }
                bytes.AddRange(data.cand[0].code);
            }
            return Encoding.GetEncoding("shift-jis").GetString(bytes.ToArray());
        }

        public void judgeResult()
        {
            double hocrAvg = GetAvgCertainty(this.HocrResultList);
            double jocrAvg = GetAvgCertainty(this.JocrResultList);
            this.ResultType = jocrAvg >= hocrAvg ? RcFontType.JOCR : RcFontType.HOCR;
            if (this.ResultType == RcFontType.HOCR)
            {
                this.ResultCharLine = GetString(this.HocrResultList);
            }
            else
            {
                this.ResultCharLine = GetString(this.JocrResultList);
            }
        }

        public Image DrawRange(Image orgImg)
        {
            Bitmap img = new Bitmap(orgImg);
            
            List<SDKResult> resultList = this.ResultType==RcFontType.HOCR?this.HocrResultList:this.JocrResultList;
            using (Graphics g = Graphics.FromImage(img))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                foreach (SDKResult ret in resultList)
                {
                    if (ret.cand[0].code[0] == 0x0a && ret.cand[0].code[1] == 0x00)
                    {
                        continue;
                    }
                    g.DrawRectangle(Pens.Red, ret.area.ToRect());
                }
            }
            return img;
        }
    }
}
