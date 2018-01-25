using Abp.Auditing;
using Cool.Normalization;
using Cool.Normalization.Auditing;

namespace Abp.Modules
{
    /// <summary>
    /// 标准输出审计日志Abp模块
    /// </summary>
    [DependsOn(typeof(NormalizationAbstractionModule))]
    public class NormalizationStdoutAuditingStoreModule : AbpModule
    {
        public override void Initialize()
        {
            if (!Configuration.Modules.Normalization()
                    .IsStandardOutputAuditLogEnabled)
            {
                return;
            }
            IocManager.RegisterAssemblyByConvention(
                typeof( NormalizationStdoutAuditingStoreModule ).Assembly );
            IocManager.ReplaceService<IAuditingStore,
                StdoutAuditingStore>();
            IocManager.ReplaceService<IResultAuditingStore,
                StdoutResultAuditingStore>();
        }

    }
}
