using DoOcrSDKInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{
    public class DoOcrExceuteResult
    {
        // <summary>
        /// 活字OCR結果
        /// </summary>
        public List<SDKResult> ResultList { get; set; }

        /// <summary>
        /// OCR結果文字列
        /// </summary>
        public string ResultCharLine {
            get
            {
                return this.GetString();
            }
        }

        public DoOcrExceuteResult(List<DoOcrSDKResult> resultList)
        {
            List < SDKResult > resList = new List<SDKResult>();
            if (resultList != null && resultList.Count > 0)
            {
                resultList.ForEach(x => resList.Add(new SDKResult(x)));
            }
            this.ResultList = resList;
        }

        private string GetString()
        {
            StringBuilder charList = new StringBuilder();
            foreach (SDKResult data in this.ResultList)
            {
                string resultChar = string.Empty;
                if (data.cand[0].code[0] == 0x0a && data.cand[0].code[1] == 0x00)
                {
                    continue;
                }
                if (data.cand[0].code[0] == 0x00 && data.cand[0].code[1] == 0x00)
                {
                    continue;
                }
                if (data.cand[0].code[0] == 0xff && data.cand[0].code[1] == 0xff)
                {
                    resultChar="^";
                }
                else
                {
                    resultChar = Encoding.GetEncoding("shift_jis").GetString(data.cand[0].code.Take(2).ToArray());
                }
                charList.Append(resultChar);
            }
            return charList.ToString();

        }

        public Image DrawRange(Image orgImg)
        {
            Bitmap img = new Bitmap(orgImg);

           using (Graphics g = Graphics.FromImage(img))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                foreach (SDKResult ret in this.ResultList)
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
