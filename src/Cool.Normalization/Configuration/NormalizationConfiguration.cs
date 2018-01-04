namespace Cool.Normalization.Configuration
{
    internal class NormalizationConfiguration : INormalizationConfiguration
    {
        private string _requestId;

        public NormalizationConfiguration()
        {
            this.UseWrapping = true;
            this.ResultAuditing = true;
            this.RequestIdHeaderName = "X-Cool-RequestId";
        }
        /// <summary>
        /// 使用规范包装输出结果
        /// </summary>
        public bool UseWrapping { get; set; }
        /// <summary>
        /// 审计
        /// </summary>
        public bool ResultAuditing { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public string RequestIdHeaderName { get => _requestId; set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new System.ArgumentNullException(nameof(value));
                }
                _requestId = value;
            } }
    }
}