using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HocrSDKInterface;
using JocrSDKInterface;
using System.Drawing;
using System.Drawing.Imaging;
using denshowSDK = DenshowNativeCommon.DenshowNativeCommon;
using DocumentSDKInterface;
using System.IO;

namespace OCRSDKTest
{

    /// <summary>
    /// 罫線処理のパラメタ変数設定
    /// </summary>
    public class EraseNoiseParams
    {

        /// <summary>
        /// ドキュメントタイプ
        /// </summary>
        public DocumentSDKDocumentType DocumentType { get; set; }
        /// <summary>
        /// レベル
        /// </summary>
        public DocumentSDKLevel Level { get; set; }
        /// <summary>
        /// ファストモード
        /// </summary>
        public DocumentSDKFastMode FastMode { get; set; }
        /// <summary>
        /// ノイズタイプ
        /// </summary>
        public DocumentSDKNoiseType NoiseType { get; set; }
        /// <summary>
        /// 最大ノイズサイズ
        /// </summary>
        public short MaxNoiseSize { get; set; }

        public bool IsAutoSize
        {
            get
            {
                return (this.MaxNoiseSize == DocumentSDKDefinition.ENSH_MDS_AUTO);
            }
        }

        public EraseNoiseParams()
        {
            this.DocumentType = DocumentSDKDocumentType.ENSH_DOT_GENERAL;
            this.Level = DocumentSDKLevel.ENSH_LVL_NORMAL;
            this.FastMode = DocumentSDKFastMode.ENSH_FTM_FINE;
            this.NoiseType = DocumentSDKNoiseType.ENSH_NST_ALL;
            this.MaxNoiseSize = DocumentSDKDefinition.ENSH_MDS_AUTO;
        }

        public DocumentSDKEnshParam ToDocumentSDKEnshParam()
        {
            DocumentSDKEnshParam param = new DocumentSDKEnshParam()
            {
                document_type = this.DocumentType,
                level = this.Level,
                fast_mode = this.FastMode,
                max_dot_size = this.IsAutoSize ? DocumentSDKDefinition.ENSH_MDS_AUTO : this.MaxNoiseSize,
                noise_type = this.NoiseType
            };
            return param;
        }
    }
    public class OcrSDK
    {
        #region NoiseEnv
        private static EraseNoiseParams _noiseEnv = null;

        public static EraseNoiseParams NoiseEnv
        {
            get
            {
                if (_noiseEnv == null)
                {
                    _noiseEnv = new EraseNoiseParams();
                    return _noiseEnv;
                }
                return _noiseEnv;

            }
        }
        /// <summary>
        /// パラメタを設定する
        /// </summary>
        public static void SetParams(EraseNoiseParams param)
        {
            _noiseEnv = param;
        }

        #endregion


        private OcrExecuteEnv _env = null;

        private OcrExecuteEnv Env
        {
            get
            {
                if (_env == null)
                {
                    _env = ocrExecuteBegin();
                }
                return _env;
            }
        }

        /// <summary>
        /// 最大認識文字数(活字、手書き共通)
        /// </summary>
        public const int MaxOcrOneLineLength = 101;

        /// <summary>
        /// OCR開始処理
        /// </summary>
        /// <returns>OCR実行環境</returns>
        public OcrExecuteEnv ocrExecuteBegin()
        {
            IHocrSDKComponent hocrSdk = HocrSDKInterface.HocrSDKFactory.GetHocrSDK();
            IJocrSDKComponent jocrSdk = JocrSDKInterface.JocrSDKFactory.GetJocrSDK();

            JocrSDKInterface.JocrSDKConfig config = new JocrSDKInterface.JocrSDKConfig();
            config.langDictTemplate[0] = Config.GetJocrLangDictPath();
            config.patternDictTemplate[0] = Config.GetJocrPatternDictPath();
            config.langDictTemplate[1] = config.langDictTemplate[2] = null;
            config.patternDictTemplate[1] = config.patternDictTemplate[2] = null;

            OcrExecuteEnv env = new OcrExecuteEnv();

            // ライブラリーの開始処理
            env.hocrLibHandle = hocrSdk.Begin();
            env.jocrLibHandle = jocrSdk.Begin(config);

            // 実行環境オープン
            env.hocrEnvHandle = hocrSdk.OpenExecEnv(env.hocrLibHandle, HocrSDKEnvType.OCR_HCR);
            env.jocrEnvHandle = jocrSdk.OpenExecEnv(env.jocrLibHandle, JocrSDKCrType.OCR_JCR);

            // 辞書ロード
            hocrSdk.LoadPatternDict(env.hocrLibHandle, Config.GetHocrPatternDictPath());
            jocrSdk.LoadPatternDict(env.jocrLibHandle, config.patternDictTemplate[0]);
            jocrSdk.LoadLangDict(env.jocrLibHandle, config.langDictTemplate[0]);

            // 辞書ハンドル取得と登録
            env.hocrPatDictHandle = hocrSdk.GetDictHandle(env.hocrLibHandle, Config.GetHocrPatternDictPath());
            hocrSdk.RegisterPatternDict(env.hocrLibHandle, env.hocrEnvHandle, env.hocrPatDictHandle);
            env.jocrPatDictHandle = jocrSdk.GetDictHandle(env.jocrLibHandle, config.patternDictTemplate[0]);
            jocrSdk.RegisterPatternDict(env.jocrLibHandle, env.jocrEnvHandle, env.jocrPatDictHandle);
            env.jocrLangDictHandle = jocrSdk.GetDictHandle(env.jocrLibHandle, config.langDictTemplate[0]);
            jocrSdk.RegisterLangDict(env.jocrLibHandle, env.jocrEnvHandle, env.jocrLangDictHandle);

            return env;
        }

        /// <summary>
        /// OCR終了処理
        /// </summary>
        /// <param name="env">OCR実行環境</param>
        public void ocrExecuteEnd()
        {

            OcrExecuteEnv env = this._env;
            if (env == null)
            {
                return;
            }

            IHocrSDKComponent hocrSdk = HocrSDKInterface.HocrSDKFactory.GetHocrSDK();
            IJocrSDKComponent jocrSdk = JocrSDKInterface.JocrSDKFactory.GetJocrSDK();

            // パターン辞書登録解除
            hocrSdk.CancelPatternDict(env.hocrLibHandle, env.hocrEnvHandle, env.hocrPatDictHandle);
            jocrSdk.CancelLangDict(env.jocrLibHandle, env.jocrEnvHandle, env.jocrLangDictHandle);
            jocrSdk.CancelPatternDict(env.jocrLibHandle, env.jocrEnvHandle, env.jocrPatDictHandle);

            // パターン辞書アンロード
            hocrSdk.UnloadPatternDict(env.hocrLibHandle, env.hocrPatDictHandle);
            jocrSdk.UnloadLangDict(env.jocrLibHandle, env.jocrLangDictHandle);
            jocrSdk.UnloadPatternDict(env.jocrLibHandle, env.jocrPatDictHandle);

            // 実行環境クローズ
            hocrSdk.CloseExecEnv(env.hocrLibHandle, env.hocrEnvHandle);
            jocrSdk.CloseExecEnv(env.jocrLibHandle, env.jocrEnvHandle);

            // ライブラリーの終了処理
            hocrSdk.End(env.hocrLibHandle);
            jocrSdk.End(env.jocrLibHandle);
        }

        /// <summary>
        /// OCR処理
        /// </summary>
        /// <param name="image">二値化画像</param>
        /// <param name="env">OCR実行環境</param>
        /// <param name="charOpt">OCR実行対象データ</param>
        /// <returns>OCR結果エンティティ</returns>
        public OcrExecuteResult executeOcr(Image image, CharOption charOpt, RcFontType mode)
        {


            OcrExecuteResult result = new OcrExecuteResult();
            OcrExecuteEnv env = this.Env;

            image = executeExtractImage(image, new Rectangle(1, 1, image.Width - 2, image.Height - 2));
            if ((mode & RcFontType.JOCR) == RcFontType.JOCR)
            {
                try
                {
                    // 活字OCR処理の実行
                    result.JocrResultList = executeJocr(image, env, charOpt);
                }
                catch
                {
                }
            }
            if ((mode & RcFontType.HOCR) == RcFontType.HOCR)
            {
                try
                {
                    //手書
                    result.HocrResultList = executeHocr(image, env, charOpt);
                }
                catch (Exception ex)
                {
                    string num = ex.Message;
                }
            }

            result.judgeResult();
            return result;
        }

        /// <summary>
        /// 活字OCR処理の実行
        /// </summary>
        /// <param name="image">二値化画像</param>
        /// <param name="env">OCR実行環境</param>
        /// <param name="ocrExecItem">OCR実行対象データ</param>
        private List<SDKResult> executeJocr(Image image, OcrExecuteEnv env, CharOption charOpt)
        {
            //// 手書きOCR処理
            //IJocrSDKComponent jocrSdk = JocrSDKInterface.JocrSDKFactory.GetJocrSDK();

            //var chartype = purseJocrParameter(charOpt);

            //// 認識対象文字種の設定
            //jocrSdk.SetCharType(env.jocrLibHandle, env.jocrPatDictHandle, chartype);

            //// 実行パラメーターの設定
            //JocrSDKInterface.JocrSDKParam param = new JocrSDKInterface.JocrSDKParam()
            //{
            //    line_dir = JocrSDKLineDir.JOCR_LD_HORIZONTAL,
            //    char_dir = JocrSDKCharDir.JOCR_CD_NORTH,
            //    edge_mode = JocrSDKEdgeMode.JOCR_EM_IGNORE,
            //    lineseg_level = JocrSDKLineSegLevel.JOCR_LL_DEFAULT,
            //    charseg_level = JocrSDKCharSegLevel.JOCR_CL_DEFAULT,
            //    quick_level = JocrSDKQuickLevel.JOCR_QL_MEDIUM,
            //    post_mode = JocrSDKPostMode.JOCR_PM_OFF,
            //    output_mode = JocrSDKOutputMode.JOCR_OM_NONE,
            //};

            //// 対象領域の設定
            //JocrSDKInterface.JocrSDKArea area = new JocrSDKInterface.JocrSDKArea()
            //{
            //    xs = 0,
            //    ys = 0,
            //    xe = System.Convert.ToInt16(image.Size.Width - 1),
            //    ye = System.Convert.ToInt16(image.Size.Height - 1),
            //};

            //// 活字OCR１行認識の実行
            //List<JocrSDKResult> jocrRetList = jocrSdk.ExecuteOneLine(env.jocrLibHandle, env.jocrEnvHandle,
            //    image, area, param, MaxOcrOneLineLength);

            //List<SDKResult> retList = new List<SDKResult>();
            //if (jocrRetList != null && jocrRetList.Count > 0)
            //{
            //    jocrRetList.ForEach(x => retList.Add(new SDKResult(x)));
            //}
            //return retList;
            return null;
        }

        /// <summary>
        /// 画像の切り出し
        /// </summary>
        /// <param name="image">入力画像</param>
        /// <param name="rect">切り出し矩形</param>
        /// <returns>切り出し画像</returns>
        private Image executeExtractImage(Image image, Rectangle rect)
        {
            Bitmap src = new Bitmap(image);

            // 切り出し画像を生成
            float resolution = float.Parse(Config.GetImageResolution());
            src.SetResolution(resolution, resolution);
            Graphics g = Graphics.FromImage(src);
            Image extractedImage = new Bitmap(rect.Width, rect.Height, g);

            // 切り出し画像の描画
            extractedImage = src.Clone(rect, PixelFormat.Format1bppIndexed);

            src.Dispose();

            return extractedImage;
        }


        /// <summary>
        /// 手書きOCR処理の実行
        /// </summary>
        /// <param name="image">二値化画像</param>
        /// <param name="env">OCR実行環境</param>
        /// <param name="ocrExecItem">OCR実行対象データ</param>
        private List<SDKResult> executeHocr(Image image, OcrExecuteEnv env, CharOption charOpt)
        {
            //// 手書きOCR処理
            //IHocrSDKComponent hocrSdk = HocrSDKInterface.HocrSDKFactory.GetHocrSDK();

            //var chartype = purseHocrParameter(charOpt);

            //// 認識対象文字種の設定
            //hocrSdk.SetCharType(env.hocrLibHandle, env.hocrPatDictHandle, chartype);

            //// 実行パラメーターの設定
            //HocrSDKInterface.HocrSDKParam param = new HocrSDKInterface.HocrSDKParam()
            //{
            //    quick_level = HocrSDKInterface.HocrSDKRecogLevel.HOCR_QL_LOW,
            //};

            //// 対象領域の設定
            //HocrSDKInterface.HocrSDKArea area = new HocrSDKInterface.HocrSDKArea()
            //{
            //    xs = 0,
            //    ys = 0,
            //    xe = System.Convert.ToInt16(image.Size.Width - 1),
            //    ye = System.Convert.ToInt16(image.Size.Height - 1),
            //};

            //// 手書きOCR１行認識の実行
            //List<HocrSDKResult> hocrRetList = hocrSdk.ExecuteOneLine(env.hocrLibHandle, env.hocrEnvHandle,
            //    image, area, param, MaxOcrOneLineLength);

            //List<SDKResult> retList = new List<SDKResult>();
            //if (hocrRetList != null && hocrRetList.Count > 0)
            //{
            //    hocrRetList.ForEach(x => retList.Add(new SDKResult(x)));
            //}
            //return retList;
            return null;
        }


        /// <summary>
        /// 手書きOCR認識対象文字種のパラメーター解析
        /// </summary>
        /// <param name="ocrExecItem">OCR実行ビュー</param>
        /// <returns>認識対象文字種</returns>
        private HocrSDKCharType purseHocrParameter(CharOption charOpt)
        {
            HocrSDKInterface.HocrSDKCharType charType = 0;

            // 数字
            if (charOpt.NoRcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_NUM;
            }

            // 英大文字
            if (charOpt.UpperCaseRcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_ALPH_UPPER;
            }
            // 英小文字はライブラリー未サポート

            // カタカナ
            if (charOpt.KatakanaRcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_KATAKANA;
            }

            // ひらがな
            if (charOpt.HiraganaRcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_HIRAGANA;
            }

            // 第1水準漢字
            if (charOpt.JISLevel1KanjiSetRcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_KANJI;
            }

            // 第2水準漢字
            if (charOpt.JISLevel2KanjiSetRcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_KANJI2;
            }

            // 記号１
            if (charOpt.Symbol1RcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_SYMBOL;
            }

            // 記号２
            if (charOpt.Symbol2RcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_SYMBOL;
            }

            // 句読点
            if (charOpt.PunctuationRcFlag)
            {
                charType |= HocrSDKCharType.HOCR_CT_SYMBOL;
            }

            return charType;
        }

        /// <summary>
        /// 活字OCR認識対象文字種のパラメーター解析
        /// </summary>
        /// <param name="ocrExecItem">OCR実行ビュー</param>
        /// <returns>認識対象文字種</returns>
        private JocrSDKInterface.JocrSDKCharType purseJocrParameter(CharOption charOpt)
        {
            JocrSDKInterface.JocrSDKCharType charType = 0;

            // 数字
            if (charOpt.NoRcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_NUM;
            }

            // 英大文字
            if (charOpt.UpperCaseRcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_ALPH_UPPER;
            }

            // 英小文字
            if (charOpt.LowerCaseRcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_ALPH_LOWER;
            }

            // カタカナ
            if (charOpt.KatakanaRcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_KATAKANA;
            }

            // ひらがな
            if (charOpt.HiraganaRcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_HIRAGANA;
            }

            // 第1水準漢字
            if (charOpt.JISLevel1KanjiSetRcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_KANJI;
            }

            // 第2水準漢字
            if (charOpt.JISLevel2KanjiSetRcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_KANJI2;
            }

            // 記号１
            if (charOpt.Symbol1RcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_SYMBOL;
            }

            // 記号２
            if (charOpt.Symbol2RcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_SYMBOL;
            }

            // 句読点
            if (charOpt.PunctuationRcFlag)
            {
                charType |= JocrSDKCharType.JOCR_CT_SYMBOL;
            }

            return charType;
        }

        /// <summary>
        /// 指定Pointから罫線を検出する
        /// </summary>
        /// <param name="orgImg"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Rectangle GetFrameByPoint(Bitmap orgImg, Point point)
        {
            Bitmap targetImg = orgImg.Clone(new Rectangle(0, 0, orgImg.Width, orgImg.Height), System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
            Rectangle rect = (Rectangle)denshowSDK.GetOneCellFrame(targetImg, point);
            return rect;
        }

        /// <summary>
        /// ノイズ除去
        /// </summary>
        /// <param name="orgImg"></param>
        /// <returns></returns>
        public static Bitmap EraseNoise(Bitmap orgImg)
        {
            IDocumentSDKComponent docSdk = DocumentSDKFactory.GetDocumentSDK();
            Bitmap targetImg = orgImg.Clone(new Rectangle(0, 0, orgImg.Width, orgImg.Height), System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
            // ノイズ除去処理
            DocumentSDKArea area = new DocumentSDKArea()
            {
                xs = 0,
                ys = 0,
                xe = targetImg.Width - 1,
                ye = targetImg.Height - 1
            };
            using (Image chgimg = docSdk.EraseNoise(targetImg, area, NoiseEnv.ToDocumentSDKEnshParam()))
            {
                return (Bitmap)chgimg.Clone();
            }
        }

        /// <summary>
        /// 画像回転
        /// </summary>
        /// <param name="orgImg"></param>
        /// <returns></returns>
        public static Bitmap RotateImage(Bitmap orgImg, int angle)
        {
            IDocumentSDKComponent docSdk = DocumentSDKFactory.GetDocumentSDK();
            Bitmap targetImg = orgImg.Clone(new Rectangle(0, 0, orgImg.Width, orgImg.Height), orgImg.PixelFormat);

            using (Image chgimg = docSdk.RotateImage(targetImg, angle))
            {
                return (Bitmap)chgimg.Clone();
            }
        }

        /// <summary>
        /// 活字OCRを使用した天地補正
        /// </summary>
        /// <param name="image">二値画像</param>
        /// <returns>天地補正後画像</returns>
        public Image ExecuteDDirRotate(Image image)
        {
            Image rotateImage = null;
            OcrExecuteEnv env = this.Env;

            try
            {
                rotateImage = JocrSDKInterface.JocrSDKFactory.GetJocrSDK().AutoRotateImage(
                    env.jocrLibHandle, env.jocrEnvHandle, env.jocrPatDictHandle,
                    image, null, JocrSDKInterface.JocrSDKDefinition.DDIR_PROCESS_CLOSE_DEFAULT);
            }
            finally
            {
                ocrExecuteEnd();
            }

            return rotateImage;
        }

        /// <summary>
        /// OMR認識処理
        /// </summary>
        /// <param name="itemImage">項目画像</param>
        /// <param name="recogItem">認識対象情報</param>
        /// <returns>OMR認識結果</returns>
        public DocumentSDKOmrResult executeOmr(Image itemImage, DocumentSDKCheckType markType, Rectangle markArea, Image masterImg, Rectangle masterRect)
        {

            IDocumentSDKComponent documentSDK = DocumentSDKFactory.GetDocumentSDK();
            FileStream stream = null;
            Image masterImage = null;
            List<DocumentSDKArea> areaList = new List<DocumentSDKArea>();
            List<DocumentSDKArea> masterAreaList = null;

            try
            {
                // OMRパラメーターの設定
                DocumentSDKOmrParam param = new DocumentSDKOmrParam()
                {
                    check_num = DocumentSDKDefinition.OMR_CN_UNKNOWN,
                    check_type = markType,
                    threshold = SByte.Parse("50"),
                    area_type = DocumentSDKAreaType.OMR_AT_SOLID,
                    around_area = DocumentSDKAroundArea.OMR_AA_UNKNOWN,
                    check_size_x = DocumentSDKDefinition.OMR_CS_UNKNOWN,
                    check_size_y = DocumentSDKDefinition.OMR_CS_UNKNOWN,
                };

                // OMR領域を設定（文字領域を抽出され場合、OCRの文字領域を使用する）
                DocumentSDKArea area = new DocumentSDKArea()
                {
                    xs = markArea.Left,
                    ys = markArea.Top,
                    xe = markArea.Left + markArea.Width - 1,
                    ye = markArea.Top + markArea.Height - 1,
                };
                areaList.Add(area);
                if (markType == DocumentSDKCheckType.OMR_CT_UNKNOWN)
                {
                    masterImage = masterImg;
                    masterAreaList = new List<DocumentSDKArea>();
                    DocumentSDKArea masterArea = new DocumentSDKArea()
                    {
                        xs = masterRect.X,
                        ys = masterRect.Y,
                        xe = masterRect.X + masterRect.Width,
                        ye = masterRect.Y + markArea.Height,
                    };
                    masterAreaList.Add(masterArea);
                }
                
                // OMR処理の実行
                DocumentSDKOmrResult result = documentSDK.ExecuteOmr(itemImage, areaList, masterImage, masterAreaList, param);
                
                return result;
            }
            finally
            {
                // ファイルストリームの破棄
                if (stream != null)
                {
                    stream.Dispose();
                }
                // マスター画像の破棄
                if (masterImage != null)
                {
                    masterImage.Dispose();
                }
            }
        }


        //public static Image ConvertToPIXELImage(Image orgImg)
        //{
        //    Image retImg =(Image) orgImg.Clone();
        //    DocumentSDKCommon.DocumentSDKCommon.TestConvertPIXELImage(ref retImg);
        //    return retImg;
        //}


        //public static Image ConvertToILSImage(Image orgImg)
        //{
        //    Image retImg = (Image)orgImg.Clone();
        //    DocumentSDKCommon.DocumentSDKCommon.TestConvertILSImage(ref retImg);
        //    return retImg;
        //}
    }


    /// <summary>
    /// 帳票認識：OCR環境エンティティ
    /// </summary>
    public class OcrExecuteEnv
    {
        /// <summary>
        /// 手書きOCRライブラリーハンドル
        /// </summary>
        public HocrSDKLibHandle hocrLibHandle { get; set; }

        /// <summary>
        /// 手書きOCR実行環境ハンドル
        /// </summary>
        public HocrSDKHandle hocrEnvHandle { get; set; }

        /// <summary>
        /// 手書きOCRパターン辞書ハンドル
        /// </summary>
        public HocrSDKHandle hocrPatDictHandle { get; set; }

        /// <summary>
        /// 活字OCRライブラリーハンドル
        /// </summary>
        public JocrSDKLibHandle jocrLibHandle { get; set; }

        /// <summary>
        /// 活字OCR実行環境ハンドル
        /// </summary>
        public JocrSDKHandle jocrEnvHandle { get; set; }

        /// <summary>
        /// 活字OCRパターン辞書ハンドル
        /// </summary>
        public JocrSDKHandle jocrPatDictHandle { get; set; }

        /// <summary>
        /// 活字OCR言語辞書ハンドル
        /// </summary>
        public JocrSDKHandle jocrLangDictHandle { get; set; }
    }



    /// <summary>
    /// 認識字体
    /// </summary>
    public enum RcFontType
    {
        None = 0,
        /// <summary>
        /// 活字OCR
        /// </summary>
        JOCR = 1,
        /// <summary>
        /// 手書OCR
        /// </summary>
        HOCR = 2
    }


}
