using Abp.AspNetCore;
using Abp.Configuration.Startup;
using Abp.Modules;
using Cool.Normalization.Permissions;

namespace Cool.Normalization
{
    [DependsOn(
        typeof( NormalizationWrappingModule ),
        typeof( NormalizationStdoutAuditingStoreModule ),
        typeof( NormalizationPermissionRemoteProxyModule )
        )]
    public class NormalizationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IAssemblyNameResolver, ConfiguableAssemblyNameResolver>();
        }
    }
}
