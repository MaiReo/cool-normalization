using Abp.Configuration.Startup;
using Cool.Normalization.Configuration;

namespace Cool.Normalization
{
    public static class StdoutAuditLogConfigurationExtensions
    {
        public static IStdoutAuditStoreConfiguration StdoutAuditLog( this IModuleConfigurations module )
            => module.AbpConfiguration.Get<IStdoutAuditStoreConfiguration>();
    }
}
