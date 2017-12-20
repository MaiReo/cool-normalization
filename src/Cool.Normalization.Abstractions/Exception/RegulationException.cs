using Cool.Normalization.Utilities;
using System;

namespace Cool.Normalization
{
    /// <summary>
    /// 表示应用程序执行过程中发生的异常
    /// </summary>
    public class NormalizationException : Exception
    {
        /// <summary>
        /// 错误详细
        /// </summary>
        public string DetailCode { get; }
        /// <summary>
        /// 错误级别
        /// 默认值 <see cref="Codes.Level.Fatal"/>
        /// </summary>
        public string LevelCode { get; }
        /// <summary>
        /// 表示应用程序执行过程中发生的异常
        /// </summary>
        /// <param name="detailCode"><seealso cref="CodePart.Detail"/>详细Code</param>
        /// <param name="levelCode"><seealso cref="CodePart.Level"/>级别Code</param>
        /// <param name="message"><seealso cref="Exception.Message"/>错误信息</param>
        public NormalizationException( string detailCode = Codes.Detail.Default,
            string levelCode = Codes.Level.Fatal,
            string message = null ) : base( message ?? "User throws an exception." )
        {
            this.DetailCode = detailCode;
            this.LevelCode = levelCode;
        }
    }
}
