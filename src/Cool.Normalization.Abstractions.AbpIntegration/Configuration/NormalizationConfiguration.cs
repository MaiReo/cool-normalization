namespace Cool.Normalization.Configuration
{
    internal class NormalizationConfiguration : INormalizationConfiguration
    {
        private string _requestId;

        public NormalizationConfiguration()
        {
            this._requestId = "X-Cool-RequestId";
            IsPermissionEnabled = true;
            IsStandardOutputAuditLogEnabled = true;
            IsWrappingEnabled = true;
        }
        /// <summary>
        /// 
        /// </summary>

        public string RequestIdHeaderName
        {
            get => _requestId; set
            {
                if (string.IsNullOrWhiteSpace( value ))
                {
                    throw new System.ArgumentNullException( nameof( value ) );
                }
                _requestId = value;
            }
        }

        public bool IsWrappingEnabled { get; set; }

        public bool IsPermissionEnabled { get; set; }

        public bool IsStandardOutputAuditLogEnabled { get; set; }

        public bool IsMessageEnabled { get; set; }

    }
}