using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Data.Odbc;
using System.IO;
using DenshowCommon.Enum;

namespace DenshowCommon
{
    /// <summary>
    /// 共通Utility関数クラス
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// fromで指定されたオブジェクトに存在するプロパティの内、toのオブジェクトに
        /// 同名のプロパティが存在した場合、値をコピーする。
        /// </summary>
        /// <remarks>
        /// <para>from及びtoの何れかがnullであった場合は値はコピーされない。</para>
        /// </remarks>
        /// <param name="from">コピー元</param>
        /// <param name="to">コピー先</param>
        public static void CopyProperty(object from, object to)
        {
            if (from == null || to == null)
            {
                return;
            }

            PropertyInfo[] properties = from.GetType().GetProperties();
            Type toType = to.GetType();
            foreach (PropertyInfo property in properties)
            {
                PropertyInfo toProperty = toType.GetProperty(property.Name);
                //Readonlyの属性に書き込みしない
                if (toProperty != null && toProperty.CanWrite)
                {
                    // Dtoのint→Entityのboolのための変換
                    if (toProperty.PropertyType.Equals(typeof(Boolean)))
                    {
                        toProperty.SetValue(to, Convert.ToBoolean(property.GetValue(from, null)), null);
                        continue;
                    }

                    toProperty.SetValue(to, property.GetValue(from, null), null);
                }
            }
        }

        /// <summary>
        /// IDのインクリメント
        /// 引数で指定されたIDと数値部分の桁数をもとに、+1されたIDを返す
        /// </summary>
        /// <param name="Id">ID</param>
        /// <param name="numberDigit">数値部分の桁数</param>
        /// <remarks>指定された桁数よりも採番した値が大きい場合、Exceptionをthrowします。</remarks>
        /// <returns>+1されたID</returns>
        public static string GetIncrementId(string Id, int numberDigit)
        {
            // IDの数値部分の定義
            long IdNumber;

            // IDの数値部分に+1
            if (long.TryParse(Id.Substring(2), out IdNumber))
            {// 整数化に失敗した場合は0を返す
                IdNumber++;
            }

            //IDNumberの対数を取得する
            int idDigit = (int)Math.Log10(IdNumber);
            //取得した対数を+1し、桁数を取得する
            idDigit++;

            //IDの桁数が指定された桁数よりも大きい場合、Exceptionを発生させる
            if (numberDigit < idDigit)
            {
                throw new Exception();
            }

            // IDの先頭2文字 + +1されたIDの数値部分を指定桁数で0埋めした文字列を代入する
            Id = Id.Substring(0, 2) + IdNumber.ToString().PadLeft(numberDigit, '0');

            return Id;
        }

        /// <summary>
        /// 現在時刻を取得
        /// </summary>
        /// <returns>現在時刻</returns>
        public static DateTime GetNow()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// ODBC接続確認
        /// </summary>
        /// <param name="dataSource">データソース名</param>
        /// <returns>True：成功　False：失敗</returns>
        public static bool CheckOdbcConenction(string dataSource)
        {
            try
            {
                string connString = CreateConnectionString(dataSource);
                using (OdbcConnection conn = new OdbcConnection(connString))
                {
                    // 接続を試みる
                    conn.Open();
                }
            }
            catch
            {
                // 接続不能
                return false;
            }
            return true;
        }

        /// <summary>
        /// データソースを元に接続文字列を返します。
        /// </summary>
        /// <param name="dataSourceName">データソース名</param>
        /// <returns>接続文字列</returns>
        public static string CreateConnectionString(string dataSourceName)
        {
            string connectString = Config.GetOdbcDataSource(dataSourceName);

            //ODBC接続オブジェクトを作成
            OdbcConnectionStringBuilder o = new OdbcConnectionStringBuilder();
            if (string.IsNullOrEmpty(connectString))
            {
                //DSNにデータソースを設定
                o.Dsn = dataSourceName;
            }
            else
            {
                //データソース定義がある場合
                o.ConnectionString = connectString;
                //接続文字列にDnsの設定がない場合、Dnsを追加する
                if (!connectString.ToLower().Contains("dns="))
                {
                    o.Dsn = dataSourceName;
                }
            }
            
            //TODO:コンフィグからID、パスワードを取得する
            return o.ConnectionString;
        }


        /// <summary>
        /// テキストがnull又は空の場合又は、0が入力されている場合trueを返します
        /// </summary>
        /// <param name="numText">判定を行う数値の文字列</param>
        /// <returns>
        /// true: null又は空又は0が入力されている場合 
        /// false:それ以外
        /// </returns>
        public static bool IsNullOrEmptyOrZero(string numText)
        {
            // 数値変換用の変数
            int tryInt;
            // テキストがnull又は空の場合又は、0が入力されている場合trueを返します
            if (string.IsNullOrEmpty(numText))
            {
                return true;
            }
            if (int.TryParse(numText, out tryInt) && tryInt == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 文字列をクウォウテイション記号で囲みます
        /// 文字列にクウォウテイション記号を含む場合、エスケープ処理する
        /// （クウォウテイション記号を二重化する）
        /// </summary>
        /// <param name="value">対象文字列</param>
        /// <param name="quotaion">クウォウテイション記号</param>
        /// <returns>クウォウテイション記号で囲み文字列</returns>
        /// <remarks>
        /// CSV出力、及びSQL文生成時、
        /// 値をクウォウテイション記号で囲み処理時使用する
        /// </remarks>
        public static string AddQuotaion(string value, string quotaion)
        {
            if (string.IsNullOrEmpty(value))
            {
                return quotaion + quotaion;
            }
            string buff = value.Replace(quotaion, quotaion + quotaion);
            return quotaion + buff + quotaion;
        }

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
    }
}
