namespace Abp.Modules
{
    [DependsOn(
        typeof( NormalizationPermissionModule ),
        typeof( NormalizationClientModule ) )]
    public class NormalizationPermissionRemoteProxyModule : AbpModule
    {

        public override void Initialize()
        {
            if (!Configuration.Modules.Normalization().IsPermissionEnabled)
            {
                return;
            }
            IocManager.RegisterAssemblyByConvention(
                typeof( NormalizationPermissionRemoteProxyModule ).Assembly );
            IocManager.RegisterAssemblyByConvention(
                typeof( cool.permission.client.Client.Configuration ).Assembly );
        }

    }
}
