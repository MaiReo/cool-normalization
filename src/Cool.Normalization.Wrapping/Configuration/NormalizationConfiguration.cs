namespace Cool.Normalization.Configuration
{
    internal class NormalizationConfiguration : INormalizationConfiguration
    {
        private string _requestId;

        public NormalizationConfiguration()
        {
            this._requestId = "X-Cool-RequestId";
        }
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