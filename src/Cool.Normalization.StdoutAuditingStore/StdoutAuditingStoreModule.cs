using Abp.Auditing;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Auditing;

namespace Cool.Normalization
{
    public class StdoutAuditingStoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //注册模块配置
            IocManager.Register<IStdoutAuditStoreConfiguration, StdoutAuditStoreConfiguration>();
        }
        public override void Initialize()
        {
            var moduleConfiguration = Configuration.Modules.StdoutAuditLog();
            if (moduleConfiguration.UseStdoutAuditStore)
            {
                // An ugly replacement
                Configuration.IocManager.IocContainer.Register(
                    Component
                    .For<IAuditingStore>()
                    .ImplementedBy<StdoutAuditingStore>()
                    .IsDefault()
                    .LifestyleSingleton()
                    );
            }
            if (moduleConfiguration.UseStdoutResultAuditStore)
            {
                // An ugly replacement
                Configuration.IocManager.IocContainer.Register(
                    Component
                    .For<IResultAuditingStore>()
                    .ImplementedBy<StdoutResultAuditingStore>()
                    .IsDefault()
                    .LifestyleSingleton()
                    );
            }
            IocManager.RegisterAssemblyByConvention( typeof(StdoutAuditingStoreModule).GetAssembly() );
        }

    }
}
