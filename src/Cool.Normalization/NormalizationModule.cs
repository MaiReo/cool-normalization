
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
