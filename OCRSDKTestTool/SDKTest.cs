using DocumentSDKComponent;
using DocumentSDKInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OCRSDKTest
{
    public partial class SDKTest : Form
    {
        public SDKTest()
        {
            InitializeComponent();
        }

        private string SelectImageFile = "";

        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(SelectImageFile) && System.IO.File.Exists(SelectImageFile))
            {
                MessageBox.Show("関数　EraseRuledLine　も削除されました");
               // string imgfile = SelectImageFile;
               // Image ocrImg =null;
               // using(Image orgImg = Image.FromFile(imgfile)){
               //     ocrImg= new Bitmap(orgImg);
               // }

               // IDocumentSDKComponent docSdk = DocumentSDKFactory.GetDocumentSDK();
               // //DocumentSDKComponent.DocumentSDKComponent docSdk = new DocumentSDKComponent.DocumentSDKComponent();
               // DocumentSDKInterface.DocumentSDKTableEnv env=new DocumentSDKTableEnv(){
               //      area=null,
               //      line_dir = LineDirType.TABLE_LD_HORIZONTAL,
               //       rl_mode=RlMode.TABLE_RM_SOLID
               // };
               // Image resultImg =ocrImg;
               // if (chkEraseLine.Checked)
               // {
               //     resultImg = docSdk.EraseRuledLine(ocrImg, env);
               //     env.line_dir = LineDirType.TABLE_LD_VERTICAL;
               //     resultImg = docSdk.EraseRuledLine(resultImg, env);
               // }
               //OcrSDK ocrSdk = new OcrSDK();
               //ocrSdk.ocrExecuteBegin();
               //CharOption opt = GetOption();


               //var result = ocrSdk.executeOcr(resultImg, opt, GetOcrMode());
               //MessageBox.Show( result.ResultCharLine);
               //this.textBox1.Text = result.ResultCharLine;
               //ocrSdk.ocrExecuteEnd();
               //Image rangeImg = result.DrawRange(resultImg);
               //this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
               //pictureBox1.Image = rangeImg;
               //pictureBox1.Tag = null;
            }
        }

        private RcFontType GetOcrMode()
        {
            RcFontType mode=RcFontType.None;
            if (this.chkJOCR.Checked)
            {
                mode = RcFontType.JOCR;
            }
            if (this.chkHCOR.Checked)
            {
                mode = mode | RcFontType.HOCR;
            }
            if (mode == RcFontType.None)
            {
                mode = RcFontType.JOCR;
            }
            return mode;
        }

        private CharOption GetOption()
        {
            CharOption opt = new CharOption()
            {
                NoRcFlag = this.chkNoRcFlag.Checked,
                UpperCaseRcFlag = this.chkUpperCaseRcFlag.Checked,
                LowerCaseRcFlag=this.chkLowerCaseRcFlag.Checked,
                HiraganaRcFlag = this.chkHiraganaRcFlag.Checked,
                KatakanaRcFlag = this.chkKatakanaRcFlag.Checked,
                JISLevel1KanjiSetRcFlag =  this.chkJISLevel1KanjiSetRcFlag.Checked,
                JISLevel2KanjiSetRcFlag = this.chkJISLevel2KanjiSetRcFlag.Checked,
                Symbol1RcFlag = this.chkSymbol1RcFlag.Checked,
                Symbol2RcFlag = this.chkSymbol2RcFlag.Checked,
                PunctuationRcFlag = this.chkPunctuationRcFlag.Checked,

            };
            return opt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            int zoom = 100;
            if (this.pictureBox1.Tag != null)
            {
                zoom = (int)this.pictureBox1.Tag;
            }
            zoom += 10;
            if (Zoomed(zoom))
            {
                this.pictureBox1.Tag = zoom;
            }
        }

        private bool Zoomed(int zoom)
        {
            if (zoom <= 100 || zoom >1000)
            {
                return false;
            }
            double scal = (double)zoom / 100;


            int w=(int)(this.pictureBox1.Image.Width*scal);
            int h=(int)(this.pictureBox1.Image.Height*scal);
            this.pictureBox1.Size = new Size(w, h);
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            int zoom = 100;
            if (this.pictureBox1.Tag != null)
            {
                zoom = (int)this.pictureBox1.Tag;
            }
            zoom = zoom - 10;
            if (Zoomed(zoom))
            {
                this.pictureBox1.Tag = zoom;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                SelectImageFile = this.openFileDialog1.FileName;
                Image ocrImg = null;
                using (Image orgImg = Image.FromFile(SelectImageFile))
                {
                    ocrImg = new Bitmap(orgImg);
                }
                this.pictureBox1.Image = ocrImg;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 tiffFrm = new Form1();
           tiffFrm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmSDKTestMain testForm = new frmSDKTestMain();
            testForm.Show();
        }



    }
}
