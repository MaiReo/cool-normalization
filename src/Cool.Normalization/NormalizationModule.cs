#region Version=1.1.4
/*
    模块引用：
    NormalizationModule
    -> NormalizationPermissionRemoteProxyModule
        -> NormalizationPermissionModule 
            -> NormalizationAbstractionModule
        -> NormalizationClientModule
    -> NormalizationStdoutAuditingStoreModule
        -> NormalizationAbstractionModule
    -> NormalizationWrappingModule
        -> NormalizationAbstractionModule
        -> AbpAspNetCoreModule
    
    
    程序集引用：
    Cool.Normalization.dll
    -> Cool.Normalization.Permissions.RemoteProxies.dll
        -> Cool.Normalization.Abstractions.AbpIntegration.dll
        -> cool.permission.client.dll(来自私有nuget源)
            -> Cool.Normalization.Client.Abstractions.dll
        -> Cool.Normalization.Permissions.dll
            -> Cool.Normalization.Abstractions.AbpIntegration.dll
                -> Cool.Normalization.Abstractions.dll
    -> Cool.Normalization.StdoutAuditingStore.dll
        -> Cool.Normalization.Abstractions.AbpIntegration.dll
            -> Cool.Normalization.Abstractions.dll
    -> Cool.Normalization.Wrapping.dll
        -> Cool.Normalization.Abstractions.AbpIntegration.dll
            -> Cool.Normalization.Abstractions.dll
    -> Cool.Normalization.SwaggerIntegration.dll
*/
#endregion Version

namespace Abp.Modules
{
    /// <summary>
    /// 依赖了其他Cool.Normalization组件模块的模块
    /// 通常只需要依赖此模块。
    /// </summary>
    [DependsOn(
        typeof( NormalizationWrappingModule ),
        typeof( NormalizationStdoutAuditingStoreModule ),
        typeof( NormalizationPermissionRemoteProxyModule )
        )]
    public class NormalizationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(
                typeof( NormalizationModule ).Assembly );
        }
    }
}
