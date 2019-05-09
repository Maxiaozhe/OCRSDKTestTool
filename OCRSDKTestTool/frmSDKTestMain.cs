using DocumentSDKInterface;
using FormSDKInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using denshowSDK = DenshowNativeCommon.DenshowNativeCommon;
using NativeSDK = DenshowNativeCommon.DenshowNativeCommon;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using System.Windows.Media.Imaging;
namespace OCRSDKTest
{
    public partial class frmSDKTestMain : Form
    {
        #region Construct
        public frmSDKTestMain()
        {
            InitializeComponent();
            this.counter = new Counter(SetTimeCount);
        }
        #endregion

        #region Fields
        private const int ThRunLineY = 16;
        private Counter counter = null;
        private Image OrgImage;
        #endregion

        #region Method

        /// <summary>
        /// 二値画像変換する
        /// </summary>
        /// <param name="targetImg"></param>
        /// <returns></returns>
        private Image MakeBinImage(Bitmap targetImg)
        {
            if (targetImg.PixelFormat == PixelFormat.Format1bppIndexed)
            {
                return targetImg;
            }
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
            Image resultImg = DocumentSDKInterface.DocumentSDKFactory.GetDocumentSDK().MakeBinByThresh(new Bitmap(targetImg), null, binParam);
            return resultImg;
        }

        /// <summary>
        /// 傾き補正処理
        /// </summary>
        /// <param name="orgImg"></param>
        /// <returns></returns>
        private Bitmap ModifyImage(Bitmap orgImg)
        {
            Counter counter = new Counter(SetSubTimeCount);
            IDocumentSDKComponent docSdk = DocumentSDKFactory.GetDocumentSDK();
            Bitmap TargetImg = CloneIMage(orgImg);
            double? angle = docSdk.GetAngle(TargetImg);
            if (angle.HasValue)
            {

                // SDK用引数の準備
                DocumentSDKInterface.DocumentSDKSkewParam param = new DocumentSDKInterface.DocumentSDKSkewParam()
                {
                    det_max = DocumentSDKInterface.DocumentSDKDefinition.SKEW_DET_MAX_DEFAULT,
                    det_mode = DocumentSDKInterface.DocumentSDKDetMode.SKEW_DET_MODE_DEFAULT,
                    correct_mode = DocumentSDKInterface.DocumentSDKCorrectMode.SKEW_COR_MODE_DEFAULT,
                    expand_mode = DocumentSDKInterface.DocumentSDKExpandMode.SKEW_EXPAND_MODE_DEFAULT,
                };
                counter.Start("傾き角度:" + angle.Value.ToString("#.#"));
                Image resultImg = docSdk.ModifyImage(TargetImg, angle.Value, param);
                counter.End();
                return (Bitmap)resultImg.Clone();
            }
            return orgImg;
        }

        /// <summary>
        /// イメージコピー
        /// </summary>
        /// <param name="orgImg"></param>
        /// <returns></returns>
        private Bitmap CloneIMage(Bitmap orgImg)
        {
            // return orgImg.Clone(new Rectangle(0, 0, orgImg.Width, orgImg.Height), PixelFormat.Format1bppIndexed);
            return (Bitmap)orgImg.Clone();
        }



        /// <summary>
        /// 領域情報を読み取りする
        /// </summary>
        /// <param name="fileName"></param>
        private void ReadFrameInfo(string fileName)
        {
            if (this.picOrg.Image != null)
            {
                List<Rectangle> frames = new List<Rectangle>();
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    var Serializer = new DataContractJsonSerializer(frames.GetType());
                    frames = (List<Rectangle>)Serializer.ReadObject(stream);
                    this.picOrg.Frames = new FrameCollection<Frame>(frames);
                    this.picOrg.Refresh();
                }
            }
        }

        /// <summary>
        /// 領域情報を保存する
        /// </summary>
        /// <param name="fileName"></param>
        private void SaveFrameInfo(string fileName)
        {
            if (this.picOrg.Frames.Items.Count > 0)
            {

                List<Rectangle> frames = this.picOrg.Frames.ToAllRects();
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                {
                    var Serializer = new DataContractJsonSerializer(frames.GetType());
                    Serializer.WriteObject(stream, frames);
                }
            }
        }
        /// <summary>
        /// 枠線除去BySDK
        /// </summary>
        /// <param name="orgImg"></param>
        /// <returns></returns>
        private Image EraseLineBySdk(Bitmap orgImg)
        {
            //Bitmap targetImg = CloneIMage(orgImg);

            //IDocumentSDKComponent docSdk = DocumentSDKFactory.GetDocumentSDK();
            //DocumentSDKInterface.DocumentSDKTableEnv env = new DocumentSDKTableEnv()
            //{
            //    area = new DocumentSDKArea() { xs = 0, ys = 0, xe = orgImg.Width - 1, ye = orgImg.Height - 1 },
            //    line_dir = LineDirType.TABLE_LD_HORIZONTAL,
            //    rl_mode = RlMode.TABLE_RM_SOLID
            //};
            //Image chgImg = docSdk.EraseRuledLine(targetImg, env);
            //env.rl_mode = RlMode.TABLE_RM_SOLID;
            //env.line_dir = LineDirType.TABLE_LD_VERTICAL;
            //chgImg = docSdk.EraseRuledLine(chgImg, env);
            //return chgImg;
            return null;
        }
        /// <summary>
        /// 帳票認識する
        /// </summary>
        /// <param name="dicFileName"></param>
        /// <returns></returns>
        private int IdentifyFormBySDK(Image imgBinarization, string dicFileName)
        {
            int certainty = 0;
            // 定型帳票処理SDKコンポーネントのインスタンス取得
            IFormSDKComponent formSdk = FormSDKInterface.FormSDKFactory.GetFormSDK();

            // 辞書名配列の生成（要素１つだけ）
            List<String> dictNameList = new List<String>();
            dictNameList.Add(dicFileName);

            // 定型帳票処理パラメーターの設定
            FormSDKInterface.FormSDKParam param = new FormSDKInterface.FormSDKParam
            {
                identify_mode = FormSDKInterface.FormSDKIdentifyMode.FORM_IDENT_FINE,
                rotate_mode = FormSDKInterface.FormSDKRotateMode.FORM_ROTATE_OFF,
                th_ident = FormSDKInterface.FormSDKDefinition.FORM_TH_IDENT,
            };

            // 確信度の算出
            FormSDKInterface.FormSDKIdentResult result = formSdk.IdentifyFormFromFile(imgBinarization, param, dictNameList);

            // 結果へ格納
            certainty = result.certainty;

            return certainty;
        }
        /// <summary>
        /// LockBitmap（未使用）
        /// </summary>
        /// <param name="orgImg"></param>
        /// <returns></returns>
        private byte[] GetBitmapDatas(Bitmap orgImg)
        {
            int width = orgImg.Width;
            int height = orgImg.Height;
            Bitmap pixelBmp = orgImg.Clone(new Rectangle(0, 0, width, height), PixelFormat.Format1bppIndexed);
            BitmapData bmpData = pixelBmp.LockBits(new Rectangle(0, 0, width, height),
                        ImageLockMode.ReadOnly, orgImg.PixelFormat);
            //// 获取图像参数  
            int stride = bmpData.Stride;        // 扫描线的宽度  
            IntPtr iptr = bmpData.Scan0;        // 获取bmpData的内存起始位置  

            int scanBytes = stride * height;    // 用stride宽度，表示这是内存区域的大小 
            int bitcount = Image.GetPixelFormatSize(orgImg.PixelFormat);
            byte[] pixelValues = new byte[scanBytes];  //为目标数组分配内存  
            //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中  
            System.Runtime.InteropServices.Marshal.Copy(iptr, pixelValues, 0, scanBytes);
            orgImg.UnlockBits(bmpData);  // 解锁内存区域 
            return pixelValues;
        }

        /// <summary>
        /// メッセージクリア
        /// </summary>
        private void ClearMessag()
        {
            for (int i = this.statusStrip1.Items.Count - 1; i >= 0; i--)
            {
                ToolStripItem item = this.statusStrip1.Items[i];
                if (item != this.lblTimeCount && item != this.lblCaption)
                {
                    this.statusStrip1.Items.Remove(item);
                }
            }
        }

        private FormSDKRectCrossInfo GetRectCrossInfo(string dicPath, IFormSDKComponent formSDK)
        {
            return formSDK.GetRectCrossInfoFromFile(dicPath);
        }
        /// <summary>
        /// 処理時間表示
        /// </summary>
        private void SetTimeCount()
        {
            string message = this.counter.GetTimeCounter().ToString("#,###");
            message += " メモリ:" + this.counter.GetLeakMem().ToString();
            this.lblTimeCount.Text = message;
        }

        /// <summary>
        /// サブ処理の時間表示
        /// </summary>
        /// <param name="message"></param>
        private void SetSubTimeCount(string message)
        {

            this.statusStrip1.Items.Add(new ToolStripStatusLabel(message));
        }

        #endregion

        #region Event
        private void tsbFile_ButtonClick(object sender, EventArgs e)
        {
            this.tsbFile.ShowDropDown();
        }
        /// <summary>
        /// OpenFile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOpenImage_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "画像ファイル|*.png;*.jpg;*.gif;*.tif";
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = this.openFileDialog1.FileName;
                string ext = System.IO.Path.GetExtension(fileName);
                Bitmap targetImg = null;
                using (Bitmap img = (Bitmap)Image.FromFile(fileName))
                {
                    picChg.Clear();
                    picOrg.Clear();
                    targetImg = CloneIMage(img);
                    // targetImg.Palette = GetPalette();
                    OrgImage = targetImg;
                    picOrg.Image = targetImg;
                    picOrg.RectCrosses = null;
                    picOrg.ShowRectCross = false;
                    picOrg.Zoom = 50;

                    Debug.WriteLine(fileName);
                    foreach (var prop in img.PropertyItems)
                    {

                        string bytes = string.Join(",", prop.Value);
                        Debug.WriteLine("Id={0},Type={1},Value={2},Len={3}", prop.Id, prop.Type, bytes, prop.Len);
                    }
                    Debug.WriteLine("Flags={0}", img.Flags);

                }
            }
        }
        private ColorPalette GetPalette()
        {
            using (Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format1bppIndexed))
            {
                ColorPalette pa = bmp.Palette;
                pa.Entries[0] = Color.FromArgb(255, 255, 255);
                pa.Entries[1] = Color.FromArgb(0, 0, 0);
                return pa;
            }
        }

        /// <summary>
        /// 画像辞書ファイルを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbDictFileOpen_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "画像辞書ファイル|*.dic";
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = this.openFileDialog1.FileName;
                string ext = System.IO.Path.GetExtension(fileName);
                var formSDK = FormSDKInterface.FormSDKFactory.GetFormSDK();
                var rectCrossInfo = this.GetRectCrossInfo(fileName, formSDK);
                this.picOrg.RectCrosses = new RectCrossInfo(rectCrossInfo);
                this.picOrg.ShowRectCross = true;
                this.picOrg.Refresh();
            }
        }
        /// <summary>
        /// 領域ファイルを開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbRgnFileOpen_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "領域ファイル|*.rng";
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = this.openFileDialog1.FileName;
                string ext = System.IO.Path.GetExtension(fileName);
                if (ext.ToLower().EndsWith("rng"))
                {
                    ReadFrameInfo(fileName);
                }
            }
        }
        /// <summary>
        /// 二値画像変換
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBinImageCreate_Click(object sender, EventArgs e)
        {
            try
            {


                ClearMessag();
                Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
                if (processImg == null) return;
                Bitmap targetImg = CloneIMage((Bitmap)processImg);
                //Bitmap targetImg = (new Bitmap(processImg)).Clone(new Rectangle(0, 0, processImg.Width-9, processImg.Height-9), processImg.PixelFormat);
                // GetBitmapDatas(targetImg);
                counter.Start();
                // targetImg.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Image chgImg = MakeBinImage(targetImg);
                counter.End();
                picOrg.Image = chgImg;// new Bitmap(chgImg);
                this.picOrg.ShowRectCross = false;
            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                string code = string.Join(",", ex.GetErrorNoList());
                MessageBox.Show(code);
            }

        }
        /// <summary>
        /// 活字OCRを使用した天地補正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExecuteDDirRotate_Click(object sender, EventArgs e)
        {
            try
            {
                ClearMessag();
                OcrSDK ocrSdk = new OcrSDK();
                using (Image targetImg = (Image)this.picOrg.Image.Clone())
                {
                    this.counter.Start("天地補正");
                    Image chgImg = ocrSdk.ExecuteDDirRotate(targetImg);
                    this.counter.End();
                    this.picOrg.Image = (Image)chgImg.Clone();
                    chgImg.Dispose();
                }
            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                string code = string.Join(",", ex.GetErrorNoList());
                MessageBox.Show(code);
            }
        }


        /// <summary>
        /// 傾き補正ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbModifyImage_Click(object sender, EventArgs e)
        {
            try
            {
                ClearMessag();
                Image processImg = this.picOrg.Image;
                if (processImg == null) return;
                Bitmap targetImg = CloneIMage((Bitmap)processImg);
                Bitmap chgimg = ModifyImage(targetImg);
                this.picOrg.Image = chgimg;
                this.picOrg.ShowRectCross = false;
            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                string code = string.Join(",", ex.GetErrorNoList());
                MessageBox.Show(code);
            }

        }
        /// <summary>
        /// 辞書作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbCreateDict_Click(object sender, EventArgs e)
        {
            if (this.picOrg.Image == null) return;
            this.imgSaveDlg.DefaultExt = "*.dic";
            this.imgSaveDlg.Filter = "画像辞書ファイル|*.dic";
            if (this.imgSaveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            try
            {
                string dicPath = this.imgSaveDlg.FileName;
                Bitmap imgDic = CloneIMage((Bitmap)this.picOrg.Image);
                var rectCrossInfo = CreateDict(imgDic, dicPath, this.picOrg.Frames.ToAllRects());
                this.picOrg.RectCrosses = new RectCrossInfo(rectCrossInfo);
                this.picOrg.ShowRectCross = true;
                this.picOrg.Refresh();
                MessageBox.Show(string.Format("矩形{0}個、十字点{1}個", this.picOrg.RectCrosses.Rects.Count, this.picOrg.RectCrosses.Crosses.Count));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 画像辞書を作成する
        /// </summary>
        /// <param name="imgDic"></param>
        /// <param name="dicPath"></param>
        /// <param name="frames"></param>
        private FormSDKRectCrossInfo CreateDict(Bitmap imgDic, string dicPath, List<Rectangle> frames)
        {
            var formSDK = FormSDKInterface.FormSDKFactory.GetFormSDK();
            // 領域データの設定
            FormSDKArea maxArea = new FormSDKArea()
            {
                xs = (short)0,
                ys = (short)0,
                xe = (short)(imgDic.Width - 1),
                ye = (short)(imgDic.Height - 1),
            };

            // 認識対象領域情報データの設定（テンプレート項目設計）
            List<FormSDKMasterInfo> masterInfoList = new List<FormSDKMasterInfo>();
            foreach (var frame in frames)
            {
                if (frame.X < 0 || frame.Y < 0 || frame.Width == 0 || frame.Height == 0
                    || frame.X + frame.Width > imgDic.Width || frame.Y + frame.Height > imgDic.Height)
                {
                    continue;
                }
                FormSDKArea area = new FormSDKArea()
                {
                    xs = (short)frame.X,
                    ys = (short)frame.Y,
                    xe = (short)(frame.X + frame.Width),
                    ye = (short)(frame.Y + frame.Height),
                };

                FormSDKMasterInfo masterInfo = new FormSDKMasterInfo()
                {
                    master_area = area,
                    area_type = FormSDKAreaType.FORM_AT_SOLID,
                    removal_mode = FormSDKRemovalMode.FORM_RM_OFF,
                };

                masterInfoList.Add(masterInfo);
            }
            if (System.IO.File.Exists(dicPath))
            {
                System.IO.File.Delete(dicPath);
            }
            // 定型帳票辞書の作成
            formSDK.CreateDict(imgDic, maxArea, masterInfoList, dicPath, null);
            return GetRectCrossInfo(dicPath, formSDK);
        }


        /// <summary>
        /// 辞書認識
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "画像辞書ファイル|*.dic";
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Image TargetImage = this.picOrg.Image;
                string dicFileName = this.openFileDialog1.FileName;
                string ext = System.IO.Path.GetExtension(dicFileName);
                if (ext.ToLower().EndsWith("dic"))
                {
                    int certainty = IdentifyFormBySDK(TargetImage, dicFileName);
                    MessageBox.Show(string.Format("認識率は{0}%", certainty));
                }
            }

        }
        /// <summary>
        /// 文字領域抽出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetCharArea_Click(object sender, EventArgs e)
        {
            Rectangle test = new Rectangle(0, 0, 0, 0);
            Rectangle test2 = new Rectangle(0, 0, 100, 100);
            if (test.Contains(test2))
            {

            }
            this.openFileDialog1.Filter = "画像辞書ファイル|*.dic";
            if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Image TargetImage = this.picOrg.Image;
                string dicFileName = this.openFileDialog1.FileName;
                string ext = System.IO.Path.GetExtension(dicFileName);
                if (ext.ToLower().EndsWith("dic"))
                {
                    List<TransArea> transAreas = ExtractCharArea(TargetImage, dicFileName);
                    this.picOrg.TranAreas = transAreas;
                    this.picOrg.Refresh();
                }
            }

        }

        private List<TransArea> ExtractCharArea(Image TargetImage, string dicFileName)
        {
            IFormSDKComponent formSdk = FormSDKInterface.FormSDKFactory.GetFormSDK();

            // 定型帳票辞書のロード
            FormSDKFormDict formDict = formSdk.LoadDict(dicFileName);

            // 定型帳票処理データの設定
            FormSDKDict dict = new FormSDKDict();
            dict.AddDict(formDict);

            // 定型帳票処理パラメーターの設定
            FormSDKParam param = new FormSDKParam()
            {
                identify_mode = FormSDKIdentifyMode.FORM_IDENT_FINE,
                rotate_mode = FormSDKRotateMode.FORM_ROTATE_OFF,
                th_ident = FormSDKDefinition.FORM_TH_IDENT,
            };

            // 認識対象領域情報データの設定（テンプレート項目設計より）
            List<FormSDKMasterInfo> masterInfoList = new List<FormSDKMasterInfo>();
            foreach (var v in this.picOrg.Frames.Items)
            {
                FormSDKArea area = new FormSDKArea()
                {
                    xs = (short)v.StartPoint.X,
                    ys = (short)v.StartPoint.Y,
                    xe = (short)v.EndPoint.X,
                    ye = (short)v.EndPoint.Y,
                };

                FormSDKMasterInfo masterInfo = new FormSDKMasterInfo()
                {
                    master_area = area,
                    area_type = FormSDKAreaType.FORM_AT_SOLID,
                    removal_mode = FormSDKRemovalMode.FORM_RM_OFF,
                };

                masterInfoList.Add(masterInfo);
            }
            // 帳票に対する認識対象文字領域の抽出処理
            counter.Start();
            var result = formSdk.ExtractCharArea(TargetImage, param, dict, masterInfoList);
            counter.End();

            List<FormSDKResult> resultList = result.resultList;
            this.picOrg.Image = result.eraseRuledLineImg;
            List<TransArea> transAreas = new List<TransArea>();
            transAreas.AddRange(resultList.Select(x => ConvertToTransArea(x)));

            return transAreas;

        }

        private TransArea ConvertToTransArea(FormSDKInterface.FormSDKResult sdkArea)
        {
            return new TransArea()
            {
                status = (int)sdkArea.status,
                TransRect = new Rectangle(
                    Math.Min((int)sdkArea.trans_area.xs, (int)sdkArea.trans_area.xe),
                    Math.Min((int)sdkArea.trans_area.ys, (int)sdkArea.trans_area.ye),
                    Math.Abs((int)(sdkArea.trans_area.xe - sdkArea.trans_area.xs)),
                    Math.Abs((int)(sdkArea.trans_area.ye - sdkArea.trans_area.ys))),
                CharRect = new Rectangle(
                   Math.Min((int)sdkArea.char_area.xs, (int)sdkArea.char_area.xe),
                   Math.Min((int)sdkArea.char_area.ys, (int)sdkArea.char_area.ye),
                   Math.Abs((int)(sdkArea.char_area.xe - sdkArea.char_area.xs)),
                   Math.Abs((int)(sdkArea.char_area.ye - sdkArea.char_area.ys)))
            };
        }
        /// <summary>
        /// EraseNoise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            ClearMessag();
            Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
            if (processImg == null) return;
            Bitmap targetImg = CloneIMage((Bitmap)processImg);
            counter.Start();
            Image chgimg = OcrSDK.EraseNoise(targetImg);
            counter.End();
            this.picOrg.Image = chgimg;
            this.picOrg.ShowRectCross = false;
        }

        /// <summary>
        /// EraseRuleLineBySDK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEraseRuleLineBySDK_Click(object sender, EventArgs e)
        {
            Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
            if (processImg == null) return;
            Bitmap targetImg = new Bitmap(processImg);
            counter.Start();
            Image chgImg = EraseLineBySdk(targetImg);
            counter.End();
            picOrg.Image = new Bitmap(chgImg);
            this.picOrg.ShowRectCross = false;
        }
        /// <summary>
        /// EraseFrameLine(doocrSDK)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEraseLinesDoocr64_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("EraseFrameLineも削除されました");
                //Image processImg = this.picOrg.Image;
                //List<Rectangle> regions = this.picOrg.Frames.ToAllRects();
                //if (processImg == null) return;
                //picChg.Frames = this.picOrg.Frames;
                //Image targetImg = this.MakeBinImage((Bitmap)processImg);
                //ClearMessag();
                //Image resultImg;

                //counter.Start();

                //resultImg = DoocrSDK.DoocrSDK.EraseFrameLine(targetImg, regions, 4);

                //counter.End();
                //picChg.Image = CloneIMage((Bitmap)resultImg);
                //picChg.Refresh();

            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                string code = string.Join(",", ex.GetErrorNoList());
                MessageBox.Show(code);
            }
        }
        /// <summary>
        /// EraseRuleLine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbEraseLines_Click(object sender, EventArgs e)
        {
            Image processImg = this.picOrg.Image;
            List<Rectangle> regions = this.picOrg.Frames.ToAllRects();
            if (processImg == null) return;
            picChg.Frames = this.picOrg.Frames;
            Bitmap targetImg = CloneIMage((Bitmap)processImg);
            ClearMessag();
            TableEraser eraser = new TableEraser(SetSubTimeCount);
            Image resultImg;
            if (!this.tsbShowRuleLine.Checked)
            {
                counter.Start();
                resultImg = eraser.EraseLines(targetImg, Color.White, regions);
                counter.End();
                picChg.Image = CloneIMage((Bitmap)resultImg);
                trackBarRight.Value = (int)picOrg.Zoom;
                picChg.Zoom = picOrg.Zoom;
                picChg.Refresh();
            }
            else
            {
                counter.Start();
                resultImg = eraser.DrawLineRects(targetImg, Color.Red);
                counter.End();
                picChg.Image = CloneIMage((Bitmap)resultImg);
                trackBarRight.Value = (int)picOrg.Zoom;
                picChg.Zoom = picOrg.Zoom;
                picChg.Refresh();
            }
        }

        /// <summary>
        /// パラメタ設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSetParams_Click(object sender, EventArgs e)
        {
            using (EraceParamSetting formSetting = new EraceParamSetting())
            {
                formSetting.ShowDialog(this);
            }
        }


        /// <summary>
        /// 左画像を保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSaveLeftImage_Click(object sender, EventArgs e)
        {
            this.imgSaveDlg.DefaultExt = "*.bmp";
            this.imgSaveDlg.Filter = "画像ファイル|*.bmp;*.tif";
            if (this.imgSaveDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = this.imgSaveDlg.FileName;
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                string ext = System.IO.Path.GetExtension(fileName);
                if (ext.ToLower().EndsWith("bmp"))
                {
                    Image img = this.picOrg.Image;
                    img.Save(fileName);
                }
                else if (ext.ToLower().EndsWith("tif"))
                {
                    Bitmap img = (Bitmap)this.picOrg.Image;
                    img.SaveAsTiff(fileName, TiffCompressOption.Lzw);
                    try
                    {
                        using (Bitmap bmp = new Bitmap(img))
                        {
                            string ngfilename = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_MemBmp.tif";
                            string ngfilepath = Path.Combine(Path.GetDirectoryName(fileName), ngfilename);
                            bmp.SaveAsTiff2(ngfilepath, TiffCompressOption.Lzw);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    string filename3 = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_Ng.tif";
                    string filepath3 = Path.Combine(Path.GetDirectoryName(fileName), filename3);
                    img.Save(filepath3, ImageFormat.Tiff);

                    //TiffCreator.SaveTiff(fileName, img);
                    //byte[] value = new byte[] { 0, 0 };
                    //img.SetPropertyItem(CreatePropertyItem(262, 3, value));
                    //string ngfilename = System.IO.Path.GetFileNameWithoutExtension(fileName) + "_NG.tif";
                    //string ngfilepath = Path.Combine(Path.GetDirectoryName(fileName), ngfilename);
                    //img.Save(ngfilepath, ImageFormat.Tiff);

                    //// シーケンスフォルダへ出力
                    //using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    //{
                    //    img.Save(fs, System.Drawing.Imaging.ImageFormat.Tiff);
                    //}

                    ////画像保存
                    //EncoderParameters encoderParams = new EncoderParameters(1);
                    //var param1 = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionNone);
                    //encoderParams.Param[0] = param1;
                    //var encoders = ImageCodecInfo.GetImageEncoders();
                    //var encoder = encoders.Where(x => x.MimeType.ToLower().Equals("image/tiff")).First();
                    //img.Save(fileName, encoder, encoderParams);
                }
            }
        }

        private PropertyItem CreatePropertyItem(int id, short type, byte[] value)
        {
            var newProp1 = (PropertyItem)Activator.CreateInstance(typeof(PropertyItem), BindingFlags.Instance | BindingFlags.NonPublic, null, new object[0], CultureInfo.InvariantCulture);
            newProp1.Id = id;
            newProp1.Type = type;
            newProp1.Value = value;
            newProp1.Len = value.Length;
            return newProp1;
        }


        /// <summary>
        /// 右画像を保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSaveRightImage_Click(object sender, EventArgs e)
        {
            this.imgSaveDlg.DefaultExt = "*.bmp";
            this.imgSaveDlg.Filter = "画像ファイル|*.bmp";
            if (this.imgSaveDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = this.imgSaveDlg.FileName;
                string ext = System.IO.Path.GetExtension(fileName);
                if (ext.ToLower().EndsWith("bmp"))
                {
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    Image img = this.picChg.Image;
                    img.Save(fileName);

                }
            }
        }
        /// <summary>
        /// 認識領域を保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbSaveRgnFile_Click(object sender, EventArgs e)
        {
            this.imgSaveDlg.DefaultExt = "*.rng";
            this.imgSaveDlg.Filter = "領域ファイル|*.rng";
            if (this.imgSaveDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = this.imgSaveDlg.FileName;
                string ext = System.IO.Path.GetExtension(fileName);
                if (ext.ToLower().EndsWith("rng"))
                {
                    SaveFrameInfo(fileName);
                }
            }
        }

        /// <summary>
        /// リセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbReset_Click(object sender, EventArgs e)
        {
            this.picOrg.Image = (Image)OrgImage.Clone();

            this.picOrg.RectCrosses = null;
            this.picChg.Clear();
            this.picChg.Visible = true;
        }

        /// <summary>
        /// フォームロードイベント処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_Load(object sender, EventArgs e)
        {
            this.picOrg.ShowFrames = this.tsbShowRegion.Checked;
            this.picChg.ShowFrames = this.tsbShowRegion.Checked;
            this.picOrg.AllowAddFrame = true;
            this.picChg.AllowAddFrame = false;
            this.picOrg.Zoom = 50;
            this.picChg.Zoom = 50;
        }

        /// <summary>
        /// 認識領域表示／非表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbShowRegion_CheckedChanged(object sender, EventArgs e)
        {
            this.picOrg.ShowFrames = this.tsbShowRegion.Checked;
            this.picChg.ShowFrames = this.tsbShowRegion.Checked;
            this.picOrg.Refresh();
            this.picChg.Refresh();
        }

        /// <summary>
        /// 認識領域クリア
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbClearRegion_Click(object sender, EventArgs e)
        {
            this.picOrg.Frames.Clear();
            this.picChg.Frames.Clear();
            this.picOrg.TranAreas = null;
            this.picChg.TranAreas = null;
            this.picOrg.Refresh();
            this.picChg.Refresh();
        }
        /// <summary>
        /// OCR実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbExecOcr_Click(object sender, EventArgs e)
        {
            using (OCRExecFrm ocrFrm = new OCRExecFrm())
            {
                List<TransArea> transAreas = null;
                this.openFileDialog1.Filter = "画像辞書ファイル|*.dic";
                if (this.openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    Image TargetImage = this.picOrg.Image;
                    string dicFileName = this.openFileDialog1.FileName;
                    string ext = System.IO.Path.GetExtension(dicFileName);
                    if (!ext.ToLower().EndsWith("dic"))
                    {
                        return;
                    }
                    transAreas = ExtractCharArea(TargetImage, dicFileName);
                    CanvasPicture targetPic = this.picChg.Image == null ? this.picOrg : this.picChg;
                    ocrFrm.Frames = targetPic.Frames.ToAllRects();
                    ocrFrm.TransAreas = transAreas;
                    ocrFrm.TargetImage = targetPic.Image;
                    ocrFrm.ShowDialog(this);
                }
            }
        }

        private void tsbOpenTiffTool_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 frmTiff = new Form1();
                frmTiff.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 領域抽出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picOrg_ExtractFrame(object sender, ExtractFrameEventArgs e)
        {
            try
            {
                Rectangle selRect = e.Selection;
                List<Rectangle> extraFrames = ExtractFrame(selRect);
                this.picOrg.Frames.AddRange(extraFrames);
                this.picOrg.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<Rectangle> ExtractFrame(Rectangle selRect)
        {
            //選択範囲内領域を削除する
            picOrg.Frames.Items.RemoveAll(x => selRect.Contains(x.Rect));
            //領域抽出

            Bitmap targetImg = CloneIMage((Bitmap)picOrg.Image);
            if (targetImg.HorizontalResolution < 200)
            {
                targetImg.SetResolution(200, 200);
            }
            counter.Start("ExtractFrame");
            short margin = (short)TableEraser.Env.ExtractFrameMargin;
            List<Rectangle> extraFrames = NativeSDK.ExtractFrame(targetImg, selRect);
            counter.End();
            return extraFrames;
        }
        /// <summary>
        /// 全領域抽出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Rectangle selRect = new Rectangle(0, 0, this.picOrg.Image.Width - 1, this.picOrg.Image.Height - 1);
                List<Rectangle> extraFrames = ExtractFrame(selRect);
                this.picOrg.Frames.AddRange(extraFrames);
                this.picOrg.Refresh();
            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                List<int> errnos = ex.GetErrorNoList();
                string strErrNo = "";
                if (errnos != null && errnos.Count > 0)
                {
                    strErrNo = string.Join(",", errnos.ConvertAll(x => x.ToString()).ToArray());
                }
                MessageBox.Show(ex.Message + "[" + strErrNo + "]");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void trackBarLeft_ValueChanged(object sender, EventArgs e)
        {
            this.picOrg.Zoom = this.trackBarLeft.Value;

        }

        private void trackBarRight_ValueChanged(object sender, EventArgs e)
        {
            this.picChg.Zoom = this.trackBarRight.Value;
        }

        private void mnuBarcodeCreator_Click(object sender, EventArgs e)
        {
            frmBarcodeCreator barCreator = new frmBarcodeCreator();
            barCreator.Show();
        }

        private void mnuRotate90_Click(object sender, EventArgs e)
        {
            try
            {
                ClearMessag();

                Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
                if (processImg == null) return;
                Bitmap targetImg = (Bitmap)MakeBinImage((Bitmap)processImg);
                counter.Start();
                Image chgimg = OcrSDK.RotateImage(targetImg, 90);
                //Image chgimg =(Image)targetImg.Clone();
                //chgimg.RotateFlip(RotateFlipType.Rotate90FlipNone);
                counter.End();
                chgimg.Save(@"c:\chgimg90.png", ImageFormat.Png);
                this.picOrg.Image = chgimg;
                this.picOrg.ShowRectCross = false;
            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                string code = string.Join(",", ex.GetErrorNoList());
                MessageBox.Show(code);
            }

        }

        private void mnuRotate180_Click(object sender, EventArgs e)
        {
            try
            {
                ClearMessag();
                Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
                if (processImg == null) return;
                Bitmap targetImg = (Bitmap)MakeBinImage((Bitmap)processImg);
                counter.Start();
                Image chgimg = OcrSDK.RotateImage(targetImg, 180);
                //Image chgimg = (Image)targetImg.Clone();  //.net bug fix
                //chgimg.RotateFlip(RotateFlipType.Rotate180FlipNone);
                counter.End();
                chgimg.Save(@"c:\chgimg180.png", ImageFormat.Png);
                this.picOrg.Image = chgimg;
                this.picOrg.ShowRectCross = false;
            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                string code = string.Join(",", ex.GetErrorNoList());
                MessageBox.Show(code);
            }
        }

        private void mnuRotate270_Click(object sender, EventArgs e)
        {
            try
            {
                ClearMessag();
                Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
                if (processImg == null) return;
                Bitmap targetImg = (Bitmap)MakeBinImage((Bitmap)processImg);
                counter.Start();
                Image chgimg = OcrSDK.RotateImage(targetImg, 270);
                //Image chgimg = (Image)targetImg.Clone();
                //chgimg.RotateFlip(RotateFlipType.Rotate270FlipNone);
                chgimg.Save(@"c:\chgimg270.png", ImageFormat.Png);
                counter.End();
                this.picOrg.Image = chgimg;
                this.picOrg.ShowRectCross = false;
            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                string code = string.Join(",", ex.GetErrorNoList());
                MessageBox.Show(code);
            }

        }

        private void mnuRotateNone_Click(object sender, EventArgs e)
        {
            try
            {
                ClearMessag();
                Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
                if (processImg == null) return;
                Bitmap targetImg = (Bitmap)MakeBinImage((Bitmap)processImg);
                counter.Start();
                Image chgimg = OcrSDK.RotateImage(targetImg, 0);
                chgimg.Save(@"c:\chgimg0.png", ImageFormat.Png);
                //Image chgimg = (Image)targetImg.Clone();
                //chgimg.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                counter.End();
                this.picOrg.Image = chgimg;
                this.picOrg.ShowRectCross = false;
            }
            catch (DocumentSDKCommon.DocumentSDKException ex)
            {
                string code = string.Join(",", ex.GetErrorNoList());
                MessageBox.Show(code);
            }

        }

        private void mnuToPIXELImage_Click(object sender, EventArgs e)
        {
            //ClearMessag();
            //Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
            //if (processImg == null) return;
            //Bitmap targetImg = (Bitmap)MakeBinImage((Bitmap)processImg);
            //counter.Start();
            //Image returnImg = OcrSDK.ConvertToPIXELImage(targetImg);
            //counter.End();
            //picOrg.Image = returnImg;// new Bitmap(chgImg);
            //this.picOrg.ShowRectCross = false;
        }

        private void mnuToILSImage_Click(object sender, EventArgs e)
        {
            //ClearMessag();
            //Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
            //if (processImg == null) return;
            //Bitmap targetImg = (Bitmap)MakeBinImage((Bitmap)processImg);
            //counter.Start();
            //Image returnImg = OcrSDK.ConvertToILSImage(targetImg);
            //counter.End();
            //picOrg.Image = returnImg;// new Bitmap(chgImg);
            //this.picOrg.ShowRectCross = false;
        }

        private void mnuDPIChange_Click(object sender, EventArgs e)
        {
            ClearMessag();
            Image processImg = this.picChg.Image == null ? this.picOrg.Image : this.picChg.Image;
            if (processImg == null) return;
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            float dpi = ((Bitmap)processImg).HorizontalResolution;
            switch (menuItem.Name)
            {
                case "mnuDPI96":
                    dpi = 96;
                    break;
                case "mnuDPI100":
                    dpi = 100;
                    break;
                case "mnuDPI200":
                    dpi = 200;
                    break;
                case "mnuDPI300":
                    dpi = 300;
                    break;
                default:
                    break;
            }
            picOrg.Image = SetResolution((Bitmap)processImg, dpi);
            this.picOrg.ShowRectCross = false;
        }

        private Bitmap SetResolution(Bitmap imgOrg, float dpi)
        {
            int width = (int)((float)imgOrg.Width * dpi / imgOrg.HorizontalResolution);
            int height = (int)((float)imgOrg.Height * dpi / imgOrg.VerticalResolution);
            Bitmap retImg = new Bitmap(imgOrg, width, height);
            retImg.SetResolution(dpi, dpi);
            return retImg;
        }

        private void saveAsTiffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // using(FileStream stream=new FileStream(@"D:\Projects\伝匠\01.Solutions\02.Libs\DenshowBusinessComponentTest\TestData\GetReport\image\888.TIF", FileMode.Open, FileAccess.Read))
            // "D:\Projects\伝匠\01.Solutions\02.Libs\DenshowBusinessComponentTest\TestData\T00008.tif"
            //using (FileStream stream = new FileStream(@"D:\Projects\伝匠\01.Solutions\02.Libs\DenshowBusinessComponentTest\TestData\GetReport\image\888.TIF", FileMode.Open, FileAccess.Read))
            counter.Start();

            using (Image bmp = Image.FromFile(@"D:\Projects\伝匠\01.Solutions\02.Libs\DenshowBusinessComponentTest\TestData\888.TIF"))
            {
                bmp.SaveAsTiff(@"c:\888.tif", TiffCompressOption.Lzw);
            }
            counter.End();

        }





    }
}
