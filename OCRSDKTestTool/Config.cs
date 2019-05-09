using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRSDKTest
{
    /// <summary>
    /// Config定義一覧（各種設定）
    /// <remarks>アプリケーションのconfigファイルを変更することでビルド後も変更可能</remarks>
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// シーケンスルートフォルダーの取得
        /// </summary>
        /// <returns>シーケンスルートフォルダーパス</returns>
        public static string GetSequenceRootFolder()
        {
            return ConfigurationManager.AppSettings["SequenceRootFolder"];
        }

        /// <summary>
        /// DB接続文字列の取得
        /// </summary>
        /// <returns>DB接続文字列</returns>
        public static string GetConnectionString()
        {

            return ConfigurationManager.AppSettings["ConnectionString"];
        }

        /// <summary>
        /// DB接続先のスキーマ名取得
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionSchema()
        {
            return ConfigurationManager.AppSettings["ConnectionSchema"];
        }

        /// <summary>
        /// 手書きOCRパターン辞書パス取得
        /// </summary>
        /// <returns>手書きOCRパターン辞書パス</returns>
        public static string GetHocrPatternDictPath()
        {
            return ConfigurationManager.AppSettings["HocrPatternDictPath"];
        }

        /// <summary>
        /// 活字OCR半角パターン辞書パス取得
        /// </summary>
        /// <returns>活字OCR半角パターン辞書パス</returns>
        public static string GetJocrHankakuDictPath()
        {
            return ConfigurationManager.AppSettings["JocrHankakuDictPath"];
        }

        /// <summary>
        /// 活字OCR言語辞書パス取得
        /// </summary>
        /// <returns>活字OCR言語辞書パス</returns>
        public static string GetJocrLangDictPath()
        {
            return ConfigurationManager.AppSettings["JocrLangDictPath"];
        }

        /// <summary>
        /// 活字OCRパターン辞書パス取得
        /// </summary>
        /// <returns>活字OCRパターン辞書パス</returns>
        public static string GetJocrPatternDictPath()
        {
            return ConfigurationManager.AppSettings["JocrPatternDictPath"];
        }

        /// <summary>
        /// 活字OCR記号パターン辞書パス取得
        /// </summary>
        /// <returns>活字OCR記号パターン辞書パス</returns>
        public static string GetJocrSynbolDictPath()
        {
            return ConfigurationManager.AppSettings["JocrSynbolDictPath"];
        }

        /// <summary>
        /// 複数イメージ分割時の解像度取得
        /// </summary>
        /// <returns>複数イメージ分割時の解像度</returns>
        public static string GetImageResolution()
        {
            return ConfigurationManager.AppSettings["ImageResolution"];
        }

        /// <summary>
        /// 二値化しきい値の取得
        /// </summary>
        /// <returns>二値化しきい値</returns>
        public static string GetBinalizationThreshold()
        {
            return ConfigurationManager.AppSettings["BinalizationThreshold"];
        }

        /// <summary>
        /// 知識辞書格納フォルダパス
        /// </summary>
        /// <returns>知識辞書格納フォルダパス</returns>
        public static string GetKnwlDictPath()
        {
            return ConfigurationManager.AppSettings["KnwlDictPath"];
        }

        /// <summary>
        /// 画像ファイル保存フォルダパス
        /// </summary>
        /// <returns>画像ファイル保存フォルダパス</returns>
        public static string GetSaveImgFilePath()
        {
            return ConfigurationManager.AppSettings["SaveImgFilePath"];
        }

        /// <summary>
        /// 定型帳票辞書保存フォルダパスの取得
        /// </summary>
        /// <returns>定型帳票辞書保存フォルダ</returns>
        public static string GetFormDictFilePath()
        {
            return ConfigurationManager.AppSettings["FormDictFilePath"];
        }

        /// <summary>
        /// システム辞書ファイルパスの取得
        /// </summary>
        /// <returns>システム辞書ファイル</returns>
        public static string GetSystemDictFilePath()
        {
            return ConfigurationManager.AppSettings["SystemDictPath"];
        }
    }
}
