using Abp.Configuration.Startup;
using Cool.Normalization.Configuration;

namespace Cool.Normalization
{
    public static class StdoutAuditingStoreConfigurationExtensions
    {
        public static IStdoutAuditStoreConfiguration StdoutAuditingStore( this IModuleConfigurations module )
            => module.AbpConfiguration.Get<IStdoutAuditStoreConfiguration>();
    }
}
