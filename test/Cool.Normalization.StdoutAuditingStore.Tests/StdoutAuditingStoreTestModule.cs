using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Core.Logging;

namespace Cool.Normalization.Tests
{
    [DependsOn(
        typeof( NormalizationWrappingTestModule ),
        typeof( NormalizationStdoutAuditingStoreModule )
        )]
    public class StdoutAuditingStoreTestModule : AbpModule
    {



        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(StdoutAuditingStoreTestModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!IocManager.IsRegistered<ILogger>())
            {
                IocManager.Register<ILogger, ConsoleLogger>();
            }
        }

    }
}