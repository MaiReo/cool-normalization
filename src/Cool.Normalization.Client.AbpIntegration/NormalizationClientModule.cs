using Cool.Normalization.Client;

namespace Abp.Modules
{
    /// <summary>
    /// 标准化微服务的客户端的Abp模块。
    /// 依赖此模块可按依赖注入的方式使用客户端。
    /// </summary>
    public class NormalizationClientModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar( 
                new SwaggerGenClientConventionRegistar() );
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( 
                typeof( NormalizationClientModule ).Assembly );
        }
    }
}
