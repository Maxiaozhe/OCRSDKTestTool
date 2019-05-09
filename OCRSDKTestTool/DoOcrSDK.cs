using DoOcrSDKInterface;
using HocrSDKInterface;
using JocrSDKInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{
    public class DoOcrSDK
    {

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
        /// OCR開始処理
        /// </summary>
        /// <returns>OCR実行環境</returns>
        public OcrExecuteEnv ocrExecuteBegin()
        {
            IHocrSDKComponent hocrSdk = HocrSDKInterface.HocrSDKFactory.GetHocrSDK();
            IJocrSDKComponent jocrSdk = JocrSDKInterface.JocrSDKFactory.GetJocrSDK();

            JocrSDKInterface.JocrSDKConfig config = new JocrSDKInterface.JocrSDKConfig();
            config.langDictTemplate[0] = System.IO.Path.GetDirectoryName(Config.GetJocrLangDictPath());
            config.patternDictTemplate[0] = System.IO.Path.GetDirectoryName(Config.GetJocrPatternDictPath());
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
        /// DoOCR処理
        /// </summary>
        /// <param name="image">二値化画像</param>
        /// <param name="env">OCR実行環境</param>
        /// <param name="ocrExecItem">OCR実行対象データ</param>
        /// <param name="monitoring">監視フォルダ定義</param>
        /// <returns>DoOCR結果エンティティのリスト</returns>
        public DoOcrExceuteResult executeDoOcr(Image image, TransArea transArea, CharOption charOpt, RcFontType mode)
        {
          
            IDoOcrSDKComponent doOcrSdk = DoOcrSDKFactory.GetDoOcrSDK();

            // 活字認識用環境の設定
            DoOcrSDKJocrEnvHandle jocrEnv;
            if ((mode & RcFontType.JOCR) == RcFontType.JOCR)
            {
                // 認識字体が活字か両方
                // ライブラリハンドル
                DoOcrSDKJocrLibHandle jocrLibHandle = new DoOcrSDKJocrLibHandle()
                {
                    pLibHandle = this.Env.jocrLibHandle.pLibHandle,
                };
                // 辞書ハンドル
                DoOcrSDKJocrHandle jocrDicHandle = new DoOcrSDKJocrHandle()
                {
                    handle = this.Env.jocrPatDictHandle.handle,
                };
                // 言語辞書ハンドル
                DoOcrSDKJocrHandle jocrLDicHandle = new DoOcrSDKJocrHandle()
                {
                    handle = this.Env.jocrLangDictHandle.handle,
                };
                jocrEnv = new DoOcrSDKJocrEnvHandle()
                {
                    libHandle = jocrLibHandle,              // ライブラリハンドル
                    dicHandle = jocrDicHandle,		        // 辞書ハンドル
                    ldicHandle = jocrLDicHandle,            // 言語辞書ハンドル
                    certainty = (short)charOpt.TypeCp,  // 確信度
                };

            }
            else
            {
                // 認識字体が手書き
                jocrEnv = null;
            }

            // 手書き認識用環境の設定
            DoOcrSDKHocrEnvHandle hocrEnv;
            if ((mode & RcFontType.HOCR) == RcFontType.HOCR)
            {
                // 認識字体が手書きか両方
                // ライブラリハンドル
                DoOcrSDKHocrLibHandle hocrLibHandle = new DoOcrSDKHocrLibHandle()
                {
                    pLibHandle = this.Env.hocrLibHandle.pLibHandle,
                };
                // 辞書ハンドル
                DoOcrSDKHocrHandle hocrDicHandle = new DoOcrSDKHocrHandle()
                {
                    handle = this.Env.hocrPatDictHandle.handle,
                };
                hocrEnv = new DoOcrSDKHocrEnvHandle()
                {
                    libHandle = hocrLibHandle,                          // ライブラリハンドル
                    dicHandle = hocrDicHandle,                          // 辞書ハンドル
                    certainty_ank = (short)charOpt.HwCpANK,         // 確信度(ANK)のしきい値
                    certainty_kana = (short)charOpt.HwCpKanaKanji,  // 確信度(かな)のしきい値
                };
            }
            else
            {
                // 認識字体が活字
                hocrEnv = null;
            }

            // 認識対象領域情報の設定（項目画像のため画像全体だが、ピッタリだとDoOCRがうまく動かないため）
            DoOcrSDKArea area = transArea.GetCharArea();

            // 認識実行パラメータの設定
            DoOcrSDKNoiseSize noiseSize = new DoOcrSDKNoiseSize()
            {
                minWidth = 0,			    // 有効な矩形の最小幅
                minHeight = 0,		        // 有効な矩形の最小高さ
                maxWidth = short.MaxValue,  // 有効な矩形の最大幅
                maxHeight = short.MaxValue, // 有効な矩形の最大高さ
            };
         

            DoOcrSDKItemMargin itemMargin = new DoOcrSDKItemMargin()
            {
                left = 0,
                top = 0,
                right = 0,
                bottom = 0,
            };
            DoOcrSDKOcrParam param = new DoOcrSDKOcrParam()
            {
                line_dir = DoOcrSDKDirection.LANDSCAPE,             // 行方向
                char_dir = DoOcrSDKCharDirection.NORTH,	            // 文字方向
                char_type = createDoOcrCharType(charOpt),		// 認識対象文字種
                symbol_flag = createDoOcrSymbolFlag(charOpt),   // 記号認識有無フラグ
                dilation_flag = 0,                                  // 手書き文字太らせフラグ（未使用）
                onechar_flag = (short)(charOpt.RcDigit==1 ? 1 : 0) ,                                   // 一画認識フラグ（未使用）
                form = charOpt.RcForm,					        // 認識フォーム
                noise_size = noiseSize,	                            // ノイズ除去サイズ（別途行っているためここでは不要）
                item_margin = itemMargin,	                        // マージン情報（未使用）
            };

            List<DoOcrSDKResult> resultList = new List<DoOcrSDKResult>();

            // 活字OCR１行認識の実行
            // 例外処理は一つ上で処理させるため、ここではtry-catchは使用しない
            resultList = doOcrSdk.DoOcrExecute(jocrEnv, hocrEnv, image, area, param, Convert.ToSByte(charOpt.RcDigit));

            // 指定した文字数分だけ結果を残す
            //            this.deleteResultByOutofRcDigit(resultList, Convert.ToInt32(ocrExecItem.RcDigit));

            return new DoOcrExceuteResult(resultList);
        }


        /// <summary>
        /// DoOCR認識対象文字種の値作成
        /// </summary>
        /// <param name="ocrExecItem">OCR実行ビュー</param>
        /// <returns>認識対象文字種</returns>
        private DoOcrSDKCharType createDoOcrCharType(CharOption charOpt)
        {
        
            DoOcrSDKCharType charType = 0;

            // 第1水準漢字
            if (charOpt.JISLevel1KanjiSetRcFlag)
            {
                charType |= DoOcrSDKCharType.KANJI1;
            }

            // 第2水準漢字
            if (charOpt.JISLevel2KanjiSetRcFlag)
            {
                charType |= DoOcrSDKCharType.KANJI2;
            }

            // ひらがな
            if (charOpt.HiraganaRcFlag)
            {
                charType |= DoOcrSDKCharType.HIRAGANA;
            }

            // カタカナ
            if (charOpt.KatakanaRcFlag)
            {
                charType |= DoOcrSDKCharType.KATAKANA;
            }

            // 数字
            if (charOpt.NoRcFlag)
            {
                charType |= DoOcrSDKCharType.NUMERAL;
            }

            // 英大文字
            if (charOpt.UpperCaseRcFlag)
            {
                charType |= DoOcrSDKCharType.ALPHABETUPPER;
            }

            // 英小文字
            if (charOpt.LowerCaseRcFlag)
            {
                charType |= DoOcrSDKCharType.ALPHABETLOWER;
            }

            return charType;
        }

        /// <summary>
        /// DoOCR記号フラグの値作成
        /// </summary>
        /// <param name="ocrExecItem">OCR実行ビュー</param>
        /// <returns>記号フラグ</returns>
        private DoOcrSDKSymbolFlag createDoOcrSymbolFlag(CharOption charOpt)
        {
           
            DoOcrSDKSymbolFlag symbolFlag = 0;

            // 記号１
            if (charOpt.Symbol1RcFlag)
            {
                symbolFlag |= DoOcrSDKSymbolFlag.SYMBOL_1;
            }

            // 記号２
            if (charOpt.Symbol2RcFlag)
            {
                symbolFlag |= DoOcrSDKSymbolFlag.SYMBOL_2;
            }

            // 句読点
            if (charOpt.PunctuationRcFlag)
            {
                symbolFlag |= DoOcrSDKSymbolFlag.SYMBOL_KANA;
            }

            return symbolFlag;
        }

    }


    public class CharOption
    {
        private string _rcForm;
        private int _hwCpKanaKanji=0;
        private int _hwCpANK=80;

        //数字
        public bool NoRcFlag { get; set; }
        //英大文字
        public bool UpperCaseRcFlag { get; set; }
        //英小文字
        public bool LowerCaseRcFlag { get; set; }
        //カタカナ
        public bool KatakanaRcFlag { get; set; }
        //ひらがな
        public bool HiraganaRcFlag { get; set; }
        //第一水準漢字
        public bool JISLevel1KanjiSetRcFlag { get; set; }
        //第二水準漢字
        public bool JISLevel2KanjiSetRcFlag { get; set; }
        //記号１
        public bool Symbol1RcFlag { get; set; }
        //記号２
        public bool Symbol2RcFlag { get; set; }
        //句読点
        public bool PunctuationRcFlag { get; set; }
        public bool ReportDscUseFlag { get; set; }
        public bool RejectFlag { get; set; }
        public bool ApFlag { get; set; }
        public bool TpFlag { get; set; }
        public bool RjFlag { get; set; }
        public string RcForm
        {
            get
            {
                if (!string.IsNullOrEmpty(_rcForm)){
                    return this._rcForm;
                }
                return new string('*', this.RcDigit);
            }
            set
            {
                this._rcForm = value;
            }
        }

        public int RcDigit { get; set; }
        public int TypeCp { get; set; }

        public int HwCpANK { get
            {
                return this._hwCpANK;
            }
            set
            {
                this._hwCpANK = value;
            }
        }

        public int HwCpKanaKanji {
            get
            {
                return this._hwCpKanaKanji;
            }
            set
            {
                this._hwCpKanaKanji = value;
            }
        }

    }
}
