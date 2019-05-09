using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Microsoft.VisualBasic.Devices;

namespace MemoryTest
{
    public partial class Form1 : Form
    {
        private MemCount counter = new MemCount();
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("------- 開始(Dispose Mode) ----");

            ComputerInfo info = new Microsoft.VisualBasic.Devices.ComputerInfo();
            string availablePhysicalMemory = string.Format("利用可能物理メモリ:,{0}", info.AvailablePhysicalMemory / 1024);
            string availableVirtualMemory = string.Format("利用可能仮想メモリ:,{0}", info.AvailableVirtualMemory / 1024);

            string[] files = System.IO.Directory.GetFiles(@"C:\Input\", "*.tif");
            foreach (string file in files)
            {
                counter.Start();
                Image orgImg = Image.FromFile(file);
                Image newImg = (Image)orgImg.Clone();
                orgImg.Dispose();
                newImg.Dispose();
                counter.End();
            }
            System.Diagnostics.Trace.WriteLine("------- ガベージ コレクター回収(Dispose Mode)------");
            counter.Start();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            counter.End();
            MessageBox.Show("完了しました");
        }
        /// <summary>
        /// Using
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("------- 開始(Using) ----");

            string[] files = System.IO.Directory.GetFiles(@"C:\Input\", "*.tif");
            foreach (string file in files)
            {
                counter.Start();
                using (Image orgImg = Image.FromFile(file))
                {
                    using (Image newImg = (Image)orgImg.Clone())
                    {

                    }
                }
                counter.End();
            }
            System.Diagnostics.Trace.WriteLine("------- ガベージ コレクター回収(Using)------");
            counter.Start();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            counter.End();
            MessageBox.Show("完了しました");
        }
        /// <summary>
        /// Not Dispose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("------- 開始(Not Dispose) ----");

            string[] files = System.IO.Directory.GetFiles(@"C:\Input\", "*.tif");
            foreach (string file in files)
            {
                counter.Start();
                Image orgImg = Image.FromFile(file);

                Image newImg = (Image)orgImg.Clone();
                counter.End();

            }
            System.Diagnostics.Trace.WriteLine("------- ガベージ コレクター回収(Not Dispose)------");
            counter.Start();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            counter.End();
            MessageBox.Show("完了しました");

        }
        /// <summary>
        /// FromStream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("------- 開始(FromStream) ----");

            string[] files = System.IO.Directory.GetFiles(@"C:\Input\", "*.tif");
            foreach (string file in files)
            {
                counter.Start();
                using (System.IO.FileStream stream = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                using (Image orgImg = Image.FromStream(stream))
                {
                    Image newImg = (Image)orgImg.Clone();
                }
                counter.End();
            }
            System.Diagnostics.Trace.WriteLine("------- ガベージ コレクター回収(FromStream) ----");
            counter.Start();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            counter.End();
            MessageBox.Show("完了しました");

        }

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("------- 開始(二値化処理) ----");

            string[] files = System.IO.Directory.GetFiles(@"C:\Input\", "*.tif");
            string outputPath = @"c:\output\";
            foreach (string file in files)
            {
                using (Image orgImg = Image.FromFile(file))
                {
                    counter.Start();
                    Image newImg = MakeBinImage(orgImg);
                    string outputfile = Path.ChangeExtension(Path.Combine(outputPath, System.IO.Path.GetFileName(file)), ".png");
                    newImg.Save(outputfile);
                    newImg.Dispose();
                    counter.End();
                }
            }
            System.Diagnostics.Trace.WriteLine("------- ガベージ コレクター回収(二値化処理) ----");
            counter.Start();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            counter.End();
            MessageBox.Show("完了しました");

        }

        /// <summary>
        /// 二値画像変換する
        /// </summary>
        /// <param name="targetImg"></param>
        /// <returns></returns>
        private Image MakeBinImage(Image targetImg)
        {

            // 二値化パラメーターの設定
            DocumentSDKInterface.DocumentSDKBinParam binParam = new DocumentSDKInterface.DocumentSDKBinParam();
            binParam.gray_mode = DocumentSDKInterface.DocumentSDKGrayMode.ODIB_GRAY_MODE_DEFAULT;
            binParam.xmag = 100;
            binParam.ymag = 100;
            binParam.filter_flag = DocumentSDKInterface.DocumentSDKFilterFlag.ODIB_BIN_PARAM_FILTER_OFF;
            binParam.bright = 50;
            binParam.char_exist = 50;
            binParam.thr = 128;
            // 二値化処理
            DocumentSDKComponent.DocumentSDKComponent sdk = new DocumentSDKComponent.DocumentSDKComponent();
            Image resultImg = sdk.MakeBinByThresh(new Bitmap(targetImg), null, binParam);
            return resultImg;
        }
        /// <summary>
        /// TIFFエンコーダー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("------- 開始(TIFFエンコーダー) ----");
            string[] files = System.IO.Directory.GetFiles(@"C:\Input\", "*.tif");
            string outputPath = @"c:\output\";
            foreach (string file in files)
            {
                //GC.Collect();
                counter.Start();
                using (Image orgImg = Image.FromFile(file))
                {
                    // TiffEncoderを作成する
                    TiffBitmapEncoder encoder = new TiffBitmapEncoder();
                    encoder.Compression = TiffCompressOption.Lzw;
                    // ページに追加する
                    using (Bitmap newBmp = new Bitmap(orgImg))
                    using (MemoryStream stream = new MemoryStream())
                    {
                        newBmp.SetResolution(orgImg.HorizontalResolution, orgImg.VerticalResolution);
                        newBmp.Save(stream, ImageFormat.Bmp);
                        BitmapFrame bmpFrame = BitmapFrame.Create(stream);
                        encoder.Frames.Add(bmpFrame);
                        string outputfile = System.IO.Path.Combine(outputPath, System.IO.Path.GetFileName(file));
                        using (FileStream fs = new FileStream(outputfile, FileMode.Create, FileAccess.Write))
                        {
                            encoder.Save(fs);
                        }
                    }
                }
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
                counter.End();
                System.Threading.Thread.Sleep(200);
            }
            counter.Start();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            counter.End();
            MessageBox.Show("完了しました");
        }

        private void button8_Click(object sender, EventArgs e)
        {
        
                if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string outputPath = @"c:\output\";
                    string fileName = openFileDialog1.FileName;
                    Image img = Image.FromFile(fileName);
                    string outputfile = System.IO.Path.Combine(outputPath, System.IO.Path.GetFileName(fileName));
                    img.Save(outputfile);
                    img.Dispose();

                }

            
        }




    }

    public class MemCount
    {
        private ulong beforeAvailablePhysicalMemory = 0;
        private ulong beforeAvailableVirtualMemory = 0;
        private ulong afterAvailablePhysicalMemory = 0;
        private ulong afterAvailableVirtualMemory = 0;

        public void Start()
        {
            ComputerInfo info = new Microsoft.VisualBasic.Devices.ComputerInfo();
            this.beforeAvailablePhysicalMemory = info.AvailablePhysicalMemory;
            this.beforeAvailableVirtualMemory = info.AvailableVirtualMemory;
            this.afterAvailablePhysicalMemory = 0;
            this.afterAvailableVirtualMemory = 0;
        }

        public void End()
        {
            ComputerInfo info = new Microsoft.VisualBasic.Devices.ComputerInfo();
            this.afterAvailablePhysicalMemory = info.AvailablePhysicalMemory;
            this.afterAvailableVirtualMemory = info.AvailableVirtualMemory;
            long physicalMemoryOffset = (long)((decimal)(this.beforeAvailablePhysicalMemory - this.afterAvailablePhysicalMemory) / (decimal)1024);
            long VirtualMemoryOffset = (long)((decimal)(this.beforeAvailableVirtualMemory - this.afterAvailableVirtualMemory) / (decimal)1024);

            System.Diagnostics.Trace.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fffff") + ",");
            System.Diagnostics.Trace.Write(string.Format("IN-利用可能物理メモリ,{0},", (long)((decimal)this.beforeAvailablePhysicalMemory / (decimal)1024)));
            System.Diagnostics.Trace.Write(string.Format("OUT-利用可能物理メモリ,{0},", (long)((decimal)this.afterAvailablePhysicalMemory / (decimal)1024)));
            //System.Diagnostics.Trace.Write(string.Format("IN-利用可能仮想メモリ,{0},", (long)((decimal)this.beforeAvailableVirtualMemory / (decimal)(1024))));
            //System.Diagnostics.Trace.Write(string.Format("OUT-利用可能仮想メモリ,{0},", (long)((decimal)this.afterAvailableVirtualMemory / (decimal)(1024))));
            System.Diagnostics.Trace.Write(string.Format("利用可能物理メモリ(差分),{0},", physicalMemoryOffset));
            System.Diagnostics.Trace.Write(string.Format("利用可能仮想メモリ(差分),{0},", VirtualMemoryOffset));
            //System.Diagnostics.Trace.Write(System.Environment.NewLine);
        }
    }

    public class TaskObj
    {
        private string fileName;
        private string outputPath;
        public TaskObj(string fileName, string outputPath)
        {
            this.fileName = fileName;
            this.outputPath = outputPath;
        }

        public void Start()
        {
            Image img = Image.FromFile(fileName);
            string outputfile = System.IO.Path.Combine(outputPath, System.IO.Path.GetFileName(fileName));
            // img.Save(outputfile);
            img.Dispose();
        }
    }
}
