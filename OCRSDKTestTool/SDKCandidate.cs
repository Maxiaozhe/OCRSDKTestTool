using DoOcrSDKInterface;
using HocrSDKInterface;
using JocrSDKInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCRSDKTest
{
    public class SDKCandidate
    {
        public byte[] code;
        public byte score;
        public SDKCandidate(HocrSDKCandidate cand)
        {
            this.code = cand.code;
            this.score = cand.score;
        }
        public SDKCandidate(JocrSDKCandidate cand)
        {
            List<byte> bytes = new List<byte>();
            foreach (var value in cand.code)
            {
                bytes.Add((byte)value);
            }
            this.code = bytes.ToArray();
            this.score = cand.score;
        }
        public SDKCandidate(DoOcrSDKCandidateChars cand)
        {
            List<byte> bytes = new List<byte>();
            foreach (var value in cand.code)
            {
                bytes.Add((byte)value);
            }
            this.code = bytes.ToArray();
        }
    }
}
