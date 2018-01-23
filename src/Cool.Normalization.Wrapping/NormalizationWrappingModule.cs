using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Configuration;
using Cool.Normalization.Filters;
using Cool.Normalization.Wrapping;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cool.Normalization
{
    [DependsOn( typeof( AbpAspNetCoreModule ) )]
    public class NormalizationWrappingModule : AbpModule
    {
        public override void PreInitialize()
        {
            //匿名审计
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;
            //注册模块配置
            IocManager.Register<INormalizationConfiguration, NormalizationConfiguration>();

            Configuration.ReplaceService<IExceptionFilter, NormalizationExceptionFilter>( DependencyLifeStyle.Transient );

            Configuration.ReplaceService<IAbpActionResultWrapperFactory, NormalizationActionResultWrapperFactory>(DependencyLifeStyle.Transient);

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationWrappingModule ).GetAssembly() );
        }
    }
}
