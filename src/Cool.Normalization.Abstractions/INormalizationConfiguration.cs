namespace Cool.Normalization.Configuration
{
    public interface INormalizationConfiguration
    {
        /// <summary>
        /// 指示是否使用规范包装的值。
        /// 默认值:true
        /// </summary>
        bool UseWrapping { get; set; }
        /// <summary>
        /// 审计包括输出参数
        /// 默认值:true
        /// </summary>
        bool ResultAuditing { get; set; }
        /// <summary>
        /// RequestId在Http头的名字
        /// 默认值:Request-Id
        /// </summary>
        string RequestIdHeaderName { get; set; }
    }
}