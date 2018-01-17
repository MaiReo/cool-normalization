namespace Cool.Normalization.Configuration
{
    public interface INormalizationConfiguration
    {
        /// <summary>
        /// RequestId在Http头的名字
        /// 默认值:X-Cool-RequestId
        /// </summary>
        string RequestIdHeaderName { get; set; }
    }
}