using Cool.Normalization.Client;

namespace Abp.Modules
{
    public class NormalizationClientModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar( new SwaggerGenClientConventionRegistar() );
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationClientModule ).Assembly );
        }
    }
}
