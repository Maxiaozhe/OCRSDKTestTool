using DoOcrSDKInterface;
using HocrSDKInterface;
using JocrSDKInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCRSDKTest
{
    public class SDKResult
    {
        public SDKArea area { get; set; }
        public SDKCandidate[] cand{get;set;}
        public int certainty;
        public byte status;
        public SDKResult()
        {

        }

        public  SDKResult(HocrSDKResult hocrResult){
            this.area = new SDKArea(hocrResult.area);
            List<SDKCandidate> cands = new List<SDKCandidate>();
            foreach (var cand in hocrResult.cand)
            {
                cands.Add(new SDKCandidate(cand));
            }
            this.cand = cands.ToArray();
            this.certainty =(int) hocrResult.certainty;
            this.status = hocrResult.status;
        }
        public SDKResult(JocrSDKResult hocrResult)
        {
            this.area = new SDKArea(hocrResult.area);
            List<SDKCandidate> cands = new List<SDKCandidate>();
            foreach (var cand in hocrResult.cand)
            {
                cands.Add(new SDKCandidate(cand));
            }
            this.cand = cands.ToArray();
            this.certainty = (int)hocrResult.certainty;
            this.status = hocrResult.status;
        }

        public SDKResult(DoOcrSDKResult hocrResult)
        {
            this.area = new SDKArea(hocrResult.area);
            List<SDKCandidate> cands = new List<SDKCandidate>();
            foreach (var cand in hocrResult.cand)
            {
                cands.Add(new SDKCandidate(cand));
            }
            this.cand = cands.ToArray();
            this.certainty = (int)hocrResult.cand[0].certainty;
        }
    }
}
