using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DenshowBusinessInterface
{
    /// <summary>
    /// 伝匠ビジネス実行中に発生するエラーを表します
    /// </summary>
    [Serializable]
    public class DenshowBusinessException : Exception
    {
        //private int messageId;
        private string message;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DenshowBusinessException()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        public DenshowBusinessException(string message)
            : base(message)
        {
            this.message = message;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="innerException"></param>
        public DenshowBusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
            this.message = message;
        }

        /// <summary>
        /// メッセージ情報を取得する
        /// </summary>
        public override string Message
        {
            get
            {
                return this.message;
            }
        }

        /// <summary>
        /// メッセージ情報を取得する
        /// </summary>
        /// <returns></returns>
        public string GetMessage()
        {
            return this.message;
        }
        /// <summary>
        /// エラー番号を取得する
        /// </summary>
        /// <remarks>
        /// メッセージの先頭に「EP999:」のような番号を付いている場合、
        /// その番号を取得する。存在しない場合、空文字を戻す
        /// </remarks>
        public string GetErrorCode()
        {
            Regex regex = new Regex(@"^[a-zA-Z]{1,2}[\d]{3}(?=[：:])");
            Match result = regex.Match(this.message);
            if (result.Success)
            {
                return result.Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
