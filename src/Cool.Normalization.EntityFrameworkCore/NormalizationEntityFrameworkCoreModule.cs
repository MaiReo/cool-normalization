using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Auditing;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Auditing;
using Cool.Normalization.EntityFrameworkCore.Auditing;

namespace Cool.Normalization
{
    [DependsOn( typeof( NormalizationModule ) )]
    public class NormalizationEntityFrameworkCoreModule : AbpModule
    {

        public override void PreInitialize()
        {
            IocManager.IocContainer.Register(

               //Component.For<IAuditingStore>()
               //.ImplementedBy<EntityFrameworkAuditingStore>()
               //.IsDefault()
               //.LifestyleTransient(),

                Component.For<IResultAuditingStore>()
                .ImplementedBy<EntityFrameworkResultAuditingStore>()
                .IsDefault()
                .LifestyleTransient()
               );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationEntityFrameworkCoreModule ).GetAssembly() );
        }
    }
}
