using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace OCRSDKTest
{
    public partial class frmBarcodeCreator : Form
    {
        public frmBarcodeCreator()
        {
            InitializeComponent();
        }
        private EncodingOptions EncodingOptions { get; set; }
        private BarcodeFrame CurrentBarcode = null;

        private void mnuOpenImage_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "画像ファイル|*.png;*.jpg;*.gif;*.tif";
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = this.openFileDialog1.FileName;
                string ext = System.IO.Path.GetExtension(fileName);
                using (Bitmap img = (Bitmap)Image.FromFile(fileName))
                {
                    this.canvasPicture1.Clear();
                    canvasPicture1.Image = new Bitmap(img);
                    this.panel1.Focus();
                    this.canvasPicture1.Zoom = 50;
                    this.trackBarLeft.Value = 50;
                    this.trackBarLeft.Enabled = true;
                }
            }
        }
        /// <summary>
        /// イメージコピー
        /// </summary>
        /// <param name="orgImg"></param>
        /// <returns></returns>
        private Bitmap CloneIMage(Bitmap orgImg)
        {
            return orgImg.Clone(new Rectangle(0, 0, orgImg.Width, orgImg.Height), orgImg.PixelFormat);
        }

        private void frmBarcodeCreator_Load(object sender, EventArgs e)
        {
            this.canvasPicture1.AllowAddFrame = true;
            this.canvasPicture1.ShowFrames = true;
            this.panel1.AutoScroll = true;
            InitBarcodeTypes();
            this.EncodingOptions = new EncodingOptions()
            {
                Height = 200,
                Width = 200
            };
            this.propertyGrid1.SelectedObject = this.EncodingOptions;
            this.pnlCreate.Enabled = false;
            this.trackBarLeft.Enabled = false;
        }

        private void InitBarcodeTypes()
        {
            var enumNames = Utility.GetNameValues(typeof(BarcodeFormat), BarcodeFormat.EAN_8);
            var supportFormats = enumNames.Where(x => MultiFormatWriter.SupportedWriters.Contains((BarcodeFormat)x.Value)).ToList();
            this.cmbEncoderType.DisplayMember = "DisplayName";
            this.cmbEncoderType.ValueMember = "Value";
            this.cmbEncoderType.DataSource = supportFormats;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CurrentBarcode == null || !this.CurrentBarcode.CanCreate)
                {
                    return;
                }
                this.EncodingOptions = this.EncodingOptions ?? new EncodingOptions();
                //this.EncodingOptions.Width = CurrentBarcode.Rect.Width;
                //this.EncodingOptions.Height = 0;
              
                var writer = new BarcodeWriter()
                {
                    Format = (BarcodeFormat)cmbEncoderType.SelectedValue,

                    Options = EncodingOptions,
                    Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(typeof(BitmapRenderer))
                };
                Bitmap orgImg = writer.Write(txtEncoderContent.Text);
                this.CurrentBarcode.BarcodeImage = orgImg;
                this.CurrentBarcode.RawText = txtEncoderContent.Text;
                this.CurrentBarcode.BarcodeFormat = (BarcodeFormat)cmbEncoderType.SelectedValue;
                this.canvasPicture1.Refresh();

            }
            catch (Exception exc)
            {
                MessageBox.Show(this, exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void canvasPicture1_CreateBarcode(object sender, FrameEventArgs e)
        {
            CurrentBarcode = e.Frame;
            if (this.EncodingOptions == null)
            {
                this.EncodingOptions = GetEncodeOptions();
            }
            this.EncodingOptions.Width = this.CurrentBarcode.Rect.Width;
            this.EncodingOptions.Height = this.CurrentBarcode.Rect.Height;
            this.propertyGrid1.SelectedObject = this.EncodingOptions;
            this.pnlCreate.Enabled = true;
        }

        private EncodingOptions GetEncodeOptions()
        {
            if (this.cmbEncoderType.SelectedItem == null)
            {
                return new EncodingOptions();
            }
            BarcodeFormat format = (BarcodeFormat)this.cmbEncoderType.SelectedValue;
      
            EncodingOptions options;
            switch (format)
            {
               case BarcodeFormat.QR_CODE:
                    options = new ZXing.QrCode.QrCodeEncodingOptions()
                    {
                        CharacterSet="UTF-8"
                    };

                  break;
               case BarcodeFormat.PDF_417:
                  options = new ZXing.PDF417.PDF417EncodingOptions()
                  {
                      CharacterSet="UTF-8"
                  };
                  break;
               case BarcodeFormat.DATA_MATRIX:
                  options = new ZXing.Datamatrix.DatamatrixEncodingOptions
                  {
                     SymbolShape = ZXing.Datamatrix.Encoder.SymbolShapeHint.FORCE_SQUARE
                  };
                  break;
               case BarcodeFormat.AZTEC:
                  options = new ZXing.Aztec.AztecEncodingOptions();
                  break;
               case BarcodeFormat.CODE_128:
                  options = new ZXing.OneD.Code128EncodingOptions();
                  break;
               default:
                  options = new EncodingOptions();
                  break;
            }
            if (this.CurrentBarcode != null)
            {
                options.Width = this.CurrentBarcode.Rect.Width;
                options.Height = this.CurrentBarcode.Rect.Height;
            }
            return options;
       
        }

        private void cmbEncoderType_SelectedIndexChanged(object sender, EventArgs e)
        {
          this.EncodingOptions = GetEncodeOptions();
          this.propertyGrid1.SelectedObject = this.EncodingOptions;
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            SaveImage(this.saveFileDialog1.FileName);
        }

        private void SaveImage(string saveFile)
        {
            Image orgimg=(Bitmap)this.canvasPicture1.Image.Clone();
            Image[] imgFrames = TiffCreator.GetImages(orgimg);
            List<Image> makeImgs = new List<Image>();
            foreach(Image img in imgFrames){
                Image makImg = new Bitmap(img);
                using (Graphics g = Graphics.FromImage(makImg))
                {
                    foreach (BarcodeFrame frame in this.canvasPicture1.Frames.Items)
                    {
                        if (frame.BarcodeImage != null)
                        {
                            g.DrawImage(frame.BarcodeImage, frame.StartPoint);
                        }
                    }
                }
                makeImgs.Add(makImg);
            }
           
            TiffCreator.TiffCompressType comType = TiffCreator.GetTiffFormat(orgimg);
            TiffCreator.CreateTiffImage(makeImgs.ToArray(), saveFile, comType);
        }

        private void trackBarLeft_ValueChanged(object sender, EventArgs e)
        {
            this.canvasPicture1.Zoom = this.trackBarLeft.Value;
            this.textBox1.Text = this.trackBarLeft.Value.ToString();
        }

        private void canvasPicture1_Click(object sender, EventArgs e)
        {
            this.panel1.Focus();
        }

        private void canvasPicture1_SelectedBarcode(object sender, FrameEventArgs e)
        {
            this.CurrentBarcode = e.Frame;
            if (this.EncodingOptions == null)
            {
                this.EncodingOptions = GetEncodeOptions();
            }
            this.EncodingOptions.Width = this.CurrentBarcode.Rect.Width;
            this.EncodingOptions.Height = this.CurrentBarcode.Rect.Height;
            this.propertyGrid1.SelectedObject = this.EncodingOptions;
            this.pnlCreate.Enabled = true;
            this.canvasPicture1.ShowToolTip(toolTip1, this.CurrentBarcode);
            if (CurrentBarcode.BarcodeFormat != 0)
            {
                this.cmbEncoderType.SelectedValue = CurrentBarcode.BarcodeFormat;
                this.txtEncoderContent.Text = CurrentBarcode.RawText;
            }

        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            int zoom=0;
            if (!int.TryParse(this.textBox1.Text, out zoom))
            {
                e.Cancel = true;
            }
            try
            {
                if (this.trackBarLeft.Maximum >= zoom && this.trackBarLeft.Minimum <= zoom)
                {
                    this.trackBarLeft.Value = zoom;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch
            {
                e.Cancel = true;
            }
        }

        private void mnuEncode_Click(object sender, EventArgs e)
        {
            Image orgImg = this.canvasPicture1.Image;
            if (orgImg == null)
            {
                MessageBox.Show(this, "画像を選択してください");
                return;
            }
            int count=0;
            if (canvasPicture1.Frames.Items.Count == 0)
            {
                Bitmap resultImg = (Bitmap)orgImg;
                BarcodeSDK.QRBarcodeReader QRReader = new BarcodeSDK.QRBarcodeReader();
                var results = QRReader.DecodeMultiple(resultImg);
                foreach (var result in results)
                {
                    if (result.IsSuccess && result.ResultRegion.HasValue)
                    {
                        Rectangle rect = result.ResultRegion.Value;
                        BarcodeFrame frame = new BarcodeFrame(result.ResultRegion.Value);
                        if (frame.IsTooSmall)
                        {
                            if (rect.Width < 8)
                            {
                                rect.Inflate(8, 0);
                            }
                            if (rect.Height < 8)
                            {
                                rect.Inflate(0, 8);
                            }
                            frame.Rect = rect;
                        }
                        frame.RawText = result.RawText;
                        frame.BarcodeFormat = result.BarcodeFormat;
                        frame.CanCreate = false;
                        this.canvasPicture1.Frames.Items.Add(frame);
                        this.canvasPicture1.Refresh();
                        count++;
                    }
                }
            }
            else
            {
                foreach (BarcodeFrame frame in this.canvasPicture1.Frames.Items)
                {
                    Bitmap resultImg = (Bitmap)orgImg;
                    if (frame.BarcodeImage==null)
                    {
                        BarcodeSDK.QRBarcodeReader QRReader = new BarcodeSDK.QRBarcodeReader();

                        Bitmap cutImg = (Bitmap)CutImage(frame.Rect, resultImg);
                        if (cutImg == null)
                        {
                            continue;
                        }
                        var result= QRReader.Decode(cutImg);
                        if (result.IsSuccess)
                        {
                            frame.RawText = result.RawText;
                            frame.BarcodeFormat = result.BarcodeFormat;
                            frame.CanCreate = false;
                            count++;
                        }
                    }
                }
            }
            MessageBox.Show(string.Format("認識完了しました。{0}件認識できました",count));
        }


        private Image CutImage(Rectangle rect, Image orgImage)
        {
            Rectangle imgRect = new Rectangle(0, 0, orgImage.Width, orgImage.Height);
            if (!imgRect.Contains(rect))
            {
                if (!imgRect.IntersectsWith(rect))
                {
                    return orgImage;
                }
                rect.Intersect(imgRect);
            }

            Bitmap newimg = (new Bitmap(orgImage)).Clone(rect, orgImage.PixelFormat);
            return newimg;
        }


        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            this.canvasPicture1.Frames.Clear();
            this.canvasPicture1.Refresh();
        }

        private void mnuTifCreater_Click(object sender, EventArgs e)
        {
            Form1 frmTiff = new Form1();
            frmTiff.Show();
        }

    
    }
}
