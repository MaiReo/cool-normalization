using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Configuration;
using Cool.Normalization.Wrapping;

namespace Cool.Normalization
{
    [DependsOn( typeof( AbpAspNetCoreModule ) )]
    public class NormalizationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //无需设置默认包装行为
            //var defaultWrapAttr = Configuration.Modules.AbpAspNetCore().DefaultWrapResultAttribute;
            //无需关闭响应成功时的默认包装
            //defaultWrapAttr.WrapOnSuccess = false;
            //无需关闭响应错误时的默认包装
            //defaultWrapAttr.WrapOnError = false;
            //关闭响应错误时的日志
            //defaultWrapAttr.LogError = false;

            //匿名审计
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;
            //注册模块配置
            IocManager.Register<INormalizationConfiguration, NormalizationConfiguration>();
        }
        public override void Initialize()
        {
            var moduleConfiguration = Configuration.Modules.Normalization();
            if (moduleConfiguration.UseWrapping)
            {
                // An ugly replacement
                Configuration.IocManager.IocContainer.Register(
                    Component
                    .For<IAbpActionResultWrapperFactory>()
                    .ImplementedBy<NormalizationActionResultWrapperFactory>()
                    .IsDefault()
                    .LifestyleTransient()
                    );
                //Donno why nothing happend.
                //Configuration.ReplaceService<IAbpActionResultWrapperFactory, normalizationActionResultWrapperFactory>( DependencyLifeStyle.Transient );
            }
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationModule ).GetAssembly() );
        }

    }
}
