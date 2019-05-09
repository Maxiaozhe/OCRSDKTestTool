using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Client.Result;
using ZXing.Common;

namespace OCRSDKTest.BarcodeSDK
{

    /// <summary>
    /// Barcode Readerのパラメタ変数設定
    /// </summary>
    public class BarcodeReaderParams
    {

        /// <summary>
        /// 画像を自動的に回転するかどうかを取得または設定します
        /// 回転は90度、180度、270度に対応しています
        /// </summary>
        public bool AutoRotate { get; set; }
        /// <summary>
        /// 元の画像に結果が見つからない場合は反転されるかどうかを取得または設定します。
        /// 注意：使用されているとデコード処理が遅くなる場合がある
        /// </summary>
        public bool TryInverted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TryHarder { get; set; }

        /// <summary>
        /// 画像は、純粋なモノクロ画像かどうかを取得または設定します
        /// </summary>
        public bool PureBarcode { get; set; }

        /// <summary>
        /// 認識するバーコード種別
        /// </summary>
        public IList<BarcodeFormat> PossibleFormats { get; set; }
        public BarcodeReaderParams()
        {
            this.AutoRotate = true;
            this.TryInverted = true;
            this.TryHarder = true;
            this.PureBarcode = false;
            this.PossibleFormats = null;
        }


    }

    /// <summary>
    /// Barcode Reader
    /// </summary>
    public class QRBarcodeReader
    {
        #region Env
        private static BarcodeReaderParams DefaultOptions = new BarcodeReaderParams();
        private BarcodeReaderParams _options = null;
        public BarcodeReaderParams Options
        {
            get
            {
                if (this._options == null)
                {
                    this._options = DefaultOptions;
                    return this._options;
                }
                return this._options;

            }
        }
        /// <summary>
        /// パラメタを設定する
        /// </summary>
        public static void SetDefaultOption(BarcodeReaderParams param)
        {
            DefaultOptions = param;
        }

        #endregion

        private BarcodeReader reader = null;

        public QRBarcodeReader()
        {
            this.reader = new BarcodeReader()
            {
                AutoRotate = Options.AutoRotate,
                TryInverted = Options.TryInverted,

                Options = new DecodingOptions()
                {
                    TryHarder = Options.TryHarder,
                    PureBarcode = Options.PureBarcode
                }
            };
        }

        public BarcodeResult Decode(Bitmap bitmap)
        {
            try
            {
                BarcodeResult result = this.Decode(bitmap, false);
                if (!result.IsSuccess)
                {
                    result = this.Decode(bitmap, true);
                }
                return result;
            }
            catch (Exception ex)
            {
                return new BarcodeResult(ex);
            }
        }

     

        private void SetOptions()
        {
            this.reader.AutoRotate = Options.AutoRotate;
            this.reader.TryInverted = Options.TryInverted;
            this.reader.Options.TryHarder = Options.TryHarder;
            this.reader.Options.PureBarcode = Options.PureBarcode;
        }

        private BarcodeResult Decode(Bitmap bitmap, bool pureBarcode)
        {
            try
            {
                Options.PureBarcode = pureBarcode;
                SetOptions();
                Result result = this.reader.Decode(bitmap);
                return new BarcodeResult(result);
            }
            catch (Exception ex)
            {
                return new BarcodeResult(ex);
            }
        }

        public IList<BarcodeResult> DecodeMultiple(Bitmap bitmap)
        {
            Result[] results = this.reader.DecodeMultiple(bitmap);
            List<BarcodeResult> lstResult = new List<BarcodeResult>();
            if (results == null)
            {
                return lstResult;
            }
            
            foreach (var result in results)
            {
                lstResult.Add(new BarcodeResult(result));
            }
            return lstResult;
        }
    }


    /// <summary>
    /// バーコード認識結果を格納する
    /// </summary>
    public class BarcodeResult
    {
        public bool IsSuccess { get; private set; }
        /// <summary>
        /// 認識された生文字列
        /// </summary>
        public string RawText { get; private set; }
        /// <summary>
        /// 解析されたテキスト
        /// </summary>
        public string ParsedText { get; private set; }
        /// <summary>
        /// 解析されたテキストの種別
        /// </summary>
        public ParsedResultType ParsedType { get; private set; }
        /// <summary>
        /// バーコードの識別領域
        /// </summary>
        public Rectangle? ResultRegion { get; private set; }
        /// <summary>
        /// バーコードの種別
        /// </summary>
        public BarcodeFormat BarcodeFormat { get; private set; }
        /// <summary>
        ///  認識されたバイナリデータ
        /// </summary>
        public byte[] RawBytes { get; private set; }
        /// <summary>
        /// 認識されたメタデータ
        /// </summary>
        public IDictionary<ResultMetadataType, object> ResultMetadata { get; private set; }
        /// <summary>
        /// タイムスタンプ
        /// </summary>
        public long Timestamp { get; private set; }
        /// <summary>
        /// 解析結果
        /// </summary>
        public ParsedResult ParsedResult { get; private set; }
        public Exception InnerException { get; private set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="result"></param>
        public BarcodeResult(Result result)
        {
            if (result == null)
            {
                this.IsSuccess = false;
                return;
            }
            this.RawText = result.Text;
            this.BarcodeFormat = result.BarcodeFormat;
            this.RawBytes = result.RawBytes;
            this.ResultMetadata = new Dictionary<ResultMetadataType, object>(result.ResultMetadata);
            this.Timestamp = result.Timestamp;
            this.ParsedResult = ResultParser.parseResult(result);
            this.ParsedText = this.ParsedResult.DisplayResult;
            this.ParsedType = this.ParsedResult.Type;
            this.ResultRegion = GetRegion(result);
            this.IsSuccess = true;
        }
        public BarcodeResult(Exception ex)
        {
            this.IsSuccess = false;
            this.InnerException = ex;

        }
        private Rectangle? GetRegion(Result result)
        {
            if (result.ResultPoints == null || result.ResultPoints.Length == 0)
            {
                return null;
            }
            try
            {
                int sx = (int)result.ResultPoints.Min(p => p.X);
                int sy = (int)result.ResultPoints.Min(p => p.Y);
                int ex = (int)result.ResultPoints.Max(p => p.X);
                int ey = (int)result.ResultPoints.Max(p => p.Y);
                return new Rectangle(sx, sy, ex - sx, ey - sy);
            }
            catch
            {
                return null;
            }
        }

    }


}
