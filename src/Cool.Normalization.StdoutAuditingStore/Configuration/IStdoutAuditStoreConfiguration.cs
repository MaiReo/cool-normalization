namespace Cool.Normalization
{
    public interface IStdoutAuditStoreConfiguration
    {
        bool UseStdoutAuditStore { get; set; }

        bool UseStdoutResultAuditStore { get; set; }

        string LogSeparator { get; set; }

        string LineReplacement { get; set; }
    }
}