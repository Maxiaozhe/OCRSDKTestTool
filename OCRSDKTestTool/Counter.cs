using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{
    /// <summary>
    /// 処理時間Counter
    /// </summary>
    public class Counter
    {
        private DateTime StartTime;
        private DateTime EndTime;
        private long beforeMem;
        private long afterMem;
        private string MethodName;
        public delegate void OutputHandler();
        public delegate void SubOutputHandler(string message);

        private OutputHandler handler = null;
        private SubOutputHandler subHandler = null;

        public double GetTimeCounter()
        {
            return EndTime.Subtract(StartTime).TotalMilliseconds;
        }

        public long GetLeakMem()
        {
            return this.afterMem - this.beforeMem;
        }
        public void Start()
        {
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            this.beforeMem = GC.GetTotalMemory(true);
            this.StartTime = DateTime.Now;
   
        }

        public void Start(string name)
        {
            this.MethodName = name;
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            this.beforeMem = GC.GetTotalMemory(true);
            this.StartTime = DateTime.Now;
        }
        public void End()
        {
            this.EndTime = DateTime.Now;

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            this.afterMem = GC.GetTotalMemory(true);

            afterMem = GC.GetTotalMemory(true);
            if (this.subHandler != null)
            {
                string message = this.MethodName + ":" + GetTimeCounter().ToString("#,##0");
                message += " メモリ:" + this.GetLeakMem().ToString();
                this.subHandler.Invoke(message);
            }
            if (this.handler != null)
            {
                this.handler.Invoke();
            }
        }
        public Counter(OutputHandler outputHandler)
        {
            this.handler = outputHandler;
        }
        public Counter(SubOutputHandler subOutputHandler)
        {
            this.subHandler = subOutputHandler;
        }
    }
}
