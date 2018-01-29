using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Auditing;
using Cool.Normalization.Configuration;
using Cool.Normalization.Filters;
using Cool.Normalization.Wrapping;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Abp.Modules
{
    [DependsOn( typeof( AbpAspNetCoreModule ),
        typeof( NormalizationAbstractionModule ) )]
    public class NormalizationWrappingModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;
        }
        public override void Initialize()
        {
            if (!Configuration.Modules.Normalization().IsWrappingEnabled)
            {
                return;
            }
           
            IocManager.ReplaceService<IExceptionFilter,
               NormalizationExceptionFilter>( DependencyLifeStyle.Transient );

            IocManager.ReplaceService<IAbpActionResultWrapperFactory,
               NormalizationActionResultWrapperFactory>(
                DependencyLifeStyle.Transient );

            IocManager.RegisterAssemblyByConvention(
               typeof( NormalizationWrappingModule ).Assembly );

        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IResultAuditingStore,
                SimpleLogResultAuditingStore>();
        }
    }
}
