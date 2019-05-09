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
    public partial class OCRExecFrm : Form
    {
        public OCRExecFrm()
        {
            InitializeComponent();
        }
        public Image TargetImage { get; set; }
        public List<Rectangle> Frames { get; set; }
        public List<TransArea> TransAreas { get; set; }

        private int SelectedRow = -1;
        private Counter counter = null;

        private void OCRExecFrm_Load(object sender, EventArgs e)
        {
            counter = new Counter(SetSubTimeCount);
            InitControls();
        }

        private void InitControls()
        {
            if (TargetImage == null) return;
            if (Frames == null || Frames.Count == 0)
            {
                Frames = new List<Rectangle>();
                Frames.Add(new Rectangle(0, 0, TargetImage.Width, TargetImage.Height));
            }
            this.tableLayoutPanel1.RowStyles.Clear();
            this.tableLayoutPanel1.RowCount = Frames.Count;
            int row = 0;
            foreach (TransArea area in TransAreas)
            {
                Image cutimg = CutImage(area, TargetImage);
                if (cutimg == null)
                {
                    continue;
                }
                Label label = new Label();
                label.Text = (row + 1).ToString();
                label.Dock = DockStyle.Fill;
                label.AutoSize = true;
                PictureBox picBox = new PictureBox();
                picBox.SizeMode = PictureBoxSizeMode.AutoSize;
                picBox.Image = cutimg;
                picBox.Tag = area;
                if (row > 0)
                {
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }
                this.tableLayoutPanel1.Controls.Add(label, 0, row);
                this.tableLayoutPanel1.Controls.Add(picBox, 1, row);
                label.Click += picBox_Click;
                picBox.Click += picBox_Click;
                row++;
            }

            this.cmbOmrType.DisplayMember = "DisplayName";
            this.cmbOmrType.ValueMember = "Value";
            this.cmbOmrType.DataSource = Utility.GetNameValues(typeof(DocumentSDKCheckType));
        }

        void picBox_Click(object sender, EventArgs e)
        {
            this.SelectedRow = tableLayoutPanel1.GetRow((Control)sender);
            tableLayoutPanel1.Refresh();
        }



        private Image CutImage(TransArea area, Image orgImage)
        {
            if (area.TransRect.Height <= 0 || area.TransRect.Width <= 0 ||
                area.TransRect.X + area.TransRect.Width > orgImage.Width ||
                area.TransRect.Y + area.TransRect.Height > orgImage.Height)
            {
                return null;
            }
            Bitmap newimg = (new Bitmap(orgImage)).Clone(area.TransRect, orgImage.PixelFormat);
            newimg.SetResolution(200, 200);
            return newimg;
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row == this.SelectedRow && e.Column == 1)
            {
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Highlight), e.CellBounds);
            }
        }

        /// <summary>
        /// OCR実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.SelectedRow != -1)
            {

                PictureBox pic = this.tableLayoutPanel1.GetControlFromPosition(1, this.SelectedRow) as PictureBox;
                if (pic == null) return;
                Image resultImg = pic.Image;
                counter.Start("ocrExecute");
                DoOcrSDK ocrSdk = new DoOcrSDK();

                ocrSdk.ocrExecuteBegin();
                CharOption opt = GetOption();

                TransArea area = (TransArea)pic.Tag;

                var result = ocrSdk.executeDoOcr(resultImg, area, opt, GetOcrMode());
                MessageBox.Show(result.ResultCharLine);
                this.txtResult.Text = result.ResultCharLine;

                ocrSdk.ocrExecuteEnd();
                counter.End();
                Image rangeImg = result.DrawRange(resultImg);
                this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Image = rangeImg;

            }
        }


        private CharOption GetOption()
        {
            CharOption opt = new CharOption()
            {
                NoRcFlag = this.chkNoRcFlag.Checked,
                UpperCaseRcFlag = this.chkUpperCaseRcFlag.Checked,
                LowerCaseRcFlag = this.chkLowerCaseRcFlag.Checked,
                HiraganaRcFlag = this.chkHiraganaRcFlag.Checked,
                KatakanaRcFlag = this.chkKatakanaRcFlag.Checked,
                JISLevel1KanjiSetRcFlag = this.chkJISLevel1KanjiSetRcFlag.Checked,
                JISLevel2KanjiSetRcFlag = this.chkJISLevel2KanjiSetRcFlag.Checked,
                Symbol1RcFlag = this.chkSymbol1RcFlag.Checked,
                Symbol2RcFlag = this.chkSymbol2RcFlag.Checked,
                PunctuationRcFlag = this.chkPunctuationRcFlag.Checked,
                RcDigit= (int)this.numRigit.Value,
                RcForm = string.IsNullOrEmpty(this.txtRcForm.Text)?string.Empty:this.txtRcForm.Text
            };
            return opt;
        }
        private RcFontType GetOcrMode()
        {
            RcFontType mode = RcFontType.None;
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
        private void SetSubTimeCount(string message)
        {
            this.statusStrip1.Items.Add(new ToolStripStatusLabel(message));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.SelectedRow != -1)
            {

                PictureBox pic = this.tableLayoutPanel1.GetControlFromPosition(1, this.SelectedRow) as PictureBox;
                if (pic == null) return;

                counter.Start("BarCode Decode");
                //Bitmap resultImg = new Bitmap(pic.Image);
                var docSdk = DocumentSDKInterface.DocumentSDKFactory.GetDocumentSDK();
                //Image chgimg = new Bitmap( pic.Image);
                //Image resultImg = docSdk.MakeBinByThresh(chgimg, null, null);
                Bitmap orgImg = (Bitmap)pic.Image;
                using (Bitmap chgimg = orgImg.Clone(new Rectangle(0, 0, orgImg.Width, orgImg.Height), orgImg.PixelFormat))
                {
                    Bitmap resultImg = chgimg;
                    BarcodeSDK.QRBarcodeReader QRReader = new BarcodeSDK.QRBarcodeReader();
                    var result = QRReader.Decode(resultImg);
                    if (result.IsSuccess)
                    {
                        this.txtResult.Text = "PureBarcode:" + QRReader.Options.PureBarcode.ToString() + "\r\n";
                        this.txtResult.Text += "バーコード種別：" + result.BarcodeFormat.ToString() + "\r\n" + result.RawText;
                        counter.End();
                        Image rangeImg;
                        if (result.ResultRegion.HasValue)
                        {
                            rangeImg = DrawRange(resultImg, result.ResultRegion.Value);

                        }
                        else
                        {
                            rangeImg = new Bitmap( resultImg);
                        }
                        this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                        pictureBox1.Image = rangeImg;
                    }
                    else
                    {
                        this.txtResult.Text = "バーコード認識できません";
                    }
                }
            }


        }


        public Image DrawRange(Image orgImg, Rectangle rect)
        {
            Bitmap img = new Bitmap(orgImg);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                g.DrawRectangle(Pens.Red, rect);
            }
            return img;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.SelectedRow != -1)
            {
                if (cmbOmrType.SelectedValue == null)
                {
                    MessageBox.Show("OMR種別を選択してください。");
                }

                PictureBox pic = this.tableLayoutPanel1.GetControlFromPosition(1, this.SelectedRow) as PictureBox;
                if (pic == null) return;

                Image resultImg = pic.Image;

                TransArea area = (TransArea)pic.Tag;

                Image masterImage = this.TargetImage;
                Rectangle masterArea = area.TransRect;

                OcrSDK ocrSdk = new OcrSDK();

                DocumentSDKCheckType omrType = (DocumentSDKCheckType)cmbOmrType.SelectedValue;

                Rectangle markArea = new Rectangle()
                {
                    X = area.CharRect.X -area.TransRect.X,
                    Y = area.CharRect.Y - area.TransRect.Y,
                    Width = area.CharRect.X - area.TransRect.X + area.CharRect.Width,
                    Height = area.CharRect.Y - area.TransRect.Y + area.CharRect.Height,
                };
                if (omrType == DocumentSDKCheckType.OMR_CT_UNKNOWN)
                {
                    markArea = new Rectangle()
                    {
                        X = 0 ,
                        Y = 0,
                        Width = area.TransRect.Width -1,
                        Height = area.TransRect.Height-1,
                    };
                }
                var result= ocrSdk.executeOmr(resultImg, omrType, markArea, masterImage, masterArea);
            
                this.txtResult.Text =string.Format("Check_res:{0} \n Score:{1}", result.one_res[0].check_res.ToString() , result.one_res[0].score) ;
            }
        }
    }
}
