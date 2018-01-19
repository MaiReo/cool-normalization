namespace Cool.Normalization
{
    public interface IStdoutAuditStoreConfiguration
    {
        string LogSeparator { get; set; }

        string LineReplacement { get; set; }
    }
}