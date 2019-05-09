using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OCRSDKTest
{
    public static class Utility
    {

        /// <summary>
        /// Enum値の表示名を取得する
        /// </summary>
        /// <param name="enumValue">Enum値</param>
        /// <returns>表示名</returns>
        /// <remarks>EnumNamesにリソースない時、値の文字列を返す</remarks>
        /// <example>
        /// Enum値の表示名を取得：
        /// <code>
        ///  string name = Definition.ItemKind.PICTURE.GetDisplayName();
        /// </code>
        /// </example>
        public static string GetDisplayName(this System.Enum enumValue)
        {
            string typeName = enumValue.GetType().Name + "_" + enumValue.ToString();
            string displayName = EnumNames.ResourceManager.GetString(typeName);
            //EnumNamesにリソースない時、値の文字列を返す
            return string.IsNullOrEmpty(displayName) ? enumValue.ToString() : displayName;
        }

        /// <summary>
        /// Enum型の画面表示用表示名と値一覧を取得する
        /// </summary>
        /// <param name="enumType">Enum型</param>
        /// <returns>表示名と値格納するEnumNameValueの配列</returns>
        /// <example>
        /// 使用方法：
        /// 1.画面コンボボックスにデータソースバインドする場合：
        /// <code>
        /// this.comboBox1.DisplayMember = "DisplayName";
        /// this.comboBox1.ValueMember = "Value";
        /// this.comboBox1.DataSource = EnumUtility.GeNameValues(typeof(Definition.ItemKind));
        /// </code>
        /// 2.初期値を設定する場合：
        /// <code>
        ///  this.comboBox1.SelectedValue = Definition.ItemKind.None;
        /// </code>
        /// 3.選択値を取得する場合：
        /// <code>
        ///  Definition.ItemKind itemKind = (Definition.ItemKind)this.comboBox1.SelectedValue;
        /// </code>
        /// </example>
        public static EnumNameValue[] GetNameValues(Type enumType)
        {

            if (!enumType.BaseType.Equals(typeof(System.Enum)))
            {
                return null;
            }
            var values = System.Enum.GetValues(enumType);
            List<EnumNameValue> names = new List<EnumNameValue>();
            foreach (System.Enum value in values)
            {
                names.Add(new EnumNameValue()
                {
                    DisplayName = value.GetDisplayName(),
                    Value = value
                });
            }
            return names.ToArray();
        }
        /// <summary>
        /// Enum型の画面表示用表示名と値一覧を取得する(除外する項目がある場合)
        /// </summary>
        /// <param name="enumType">Enum型</param>
        /// <param name="excludeValues">除外する値</param>
        /// <returns>表示名と値格納するEnumNameValueの配列</returns>
        /// <example>
        /// 表示項目に除外する項目がある場合
        /// <code>
        /// this.comboBox1.DataSource = EnumUtility.GeNameValues(typeof(Definition.ItemKind),Definition.ItemKind.Label);
        /// </code>
        /// </example>
        public static EnumNameValue[] GetNameValues(Type enumType, params System.Enum[] excludeValues)
        {

            if (!enumType.BaseType.Equals(typeof(System.Enum)))
            {
                return null;
            }
            var values = System.Enum.GetValues(enumType);
            List<EnumNameValue> names = new List<EnumNameValue>();
            foreach (System.Enum value in values)
            {
                if (!excludeValues.Contains(value))
                {
                    names.Add(new EnumNameValue()
                    {
                        DisplayName = value.GetDisplayName(),
                        Value = value
                    });
                }
            }
            return names.ToArray();
        }
        /// <summary>
        /// TIFF形式画像を出力する
        /// </summary>
        /// <param name="outputImg"></param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="compress">圧縮の種類</param>
        public static void SaveAsTiff(this Image outputImg, string fileName, TiffCompressOption compress)
        {
            // TiffEncoderを作成する
            TiffBitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Compression = compress;
            // ページに追加する
            string tempFileName = System.IO.Path.GetTempFileName();
            outputImg.Save(tempFileName);
            BitmapFrame bmpFrame = BitmapFrame.Create(new Uri(tempFileName), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            encoder.Frames.Add(bmpFrame);
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                encoder.Save(fs);
            }
            encoder.Dispatcher.InvokeShutdown();
            System.IO.File.Delete(tempFileName);

        }

        static void Dispatcher_ShutdownFinished(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// TIFF形式画像を出力する
        /// </summary>
        /// <param name="outputImg"></param>
        /// <param name="fileName">ファイル名</param>
        /// <param name="compress">圧縮の種類</param>
        public static void SaveAsTiff2(this Image outputImg, string fileName, TiffCompressOption compress)
        {
            // TiffEncoderを作成する
            TiffBitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Compression = compress;
            // ページに追加する
            string tempFileName = System.IO.Path.GetTempFileName();
            using (FileStream imgStream = new FileStream(tempFileName, FileMode.Create, FileAccess.ReadWrite))
            {
                outputImg.Save(imgStream, ImageFormat.Bmp);
            }
            using (FileStream stream = new FileStream(tempFileName, FileMode.Open, FileAccess.Read))
            {
                BitmapFrame bmpFrame = BitmapFrame.Create(stream);
                encoder.Frames.Add(bmpFrame);
                using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(fs);
                }
            }
        }
    }
    /// <summary>
    /// Enum型の表示名と値を格納するクラス
    /// </summary>
    public class EnumNameValue
    {
        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Enumの値
        /// </summary>
        public System.Enum Value { get; set; }
    }
}
