using Abp.Auditing;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Auditing;
using Abp.Configuration.Startup;

namespace Cool.Normalization
{
    public class NormalizationStdoutAuditingStoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //注册模块配置
            IocManager.Register<IStdoutAuditStoreConfiguration, StdoutAuditStoreConfiguration>();
            Configuration.ReplaceService<IAuditingStore, StdoutAuditingStore>();
            Configuration.ReplaceService<IResultAuditingStore, StdoutResultAuditingStore>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationStdoutAuditingStoreModule ).Assembly );
        }

    }
}
