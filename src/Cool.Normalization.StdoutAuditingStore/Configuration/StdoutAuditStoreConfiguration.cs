namespace Cool.Normalization
{
    public class StdoutAuditStoreConfiguration : IStdoutAuditStoreConfiguration
    {
        public const string DEFAULT_LOG_SEPARATOR = "|";

        public const string DEFAULT_LINE_REPLACEMENT = "/";

        public StdoutAuditStoreConfiguration()
        {
            UseStdoutAuditStore = true;
            UseStdoutResultAuditStore = true;
            LogSeparator = DEFAULT_LOG_SEPARATOR;
            LineReplacement = DEFAULT_LINE_REPLACEMENT;
        }
        public bool UseStdoutAuditStore { get; set; }

        public bool UseStdoutResultAuditStore { get; set; }

        public string LogSeparator { get; set; }

        public string LineReplacement { get; set; }
        
    }
}