namespace Cool.Normalization.Configuration
{
    public class StdoutAuditStoreConfiguration : IStdoutAuditStoreConfiguration
    {
        public const string DEFAULT_LOG_SEPARATOR = "^";

        public const string DEFAULT_LINE_REPLACEMENT = "/";

        public StdoutAuditStoreConfiguration()
        {
            LogSeparator = DEFAULT_LOG_SEPARATOR;
            LineReplacement = DEFAULT_LINE_REPLACEMENT;
        }

        public string LogSeparator { get; set; }

        public string LineReplacement { get; set; }
        
    }
}