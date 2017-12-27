using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Cool.Normalization.Messages.Tests
{
    [DependsOn(
        typeof( NormalizationMessageModule ),
        typeof( Abp.TestBase.AbpTestBaseModule )
        )]
    public class MessageTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            //RegisterIfNot<Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager>();
           
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( MessageTestModule ).GetAssembly() );
        }
       
    }
}