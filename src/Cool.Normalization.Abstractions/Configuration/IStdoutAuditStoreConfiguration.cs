namespace Cool.Normalization.Configuration
{
    /// <summary>
    /// 标准输出审计日志的相关配置
    /// </summary>
    public interface IStdoutAuditStoreConfiguration
    {
        /// <summary>
        /// 日志分隔符。
        /// </summary>
        string LogSeparator { get; set; }
        /// <summary>
        /// 换行替换符。
        /// </summary>
        string LineReplacement { get; set; }
    }
}