using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using normalizationtests.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using normalizationtests.Localization;
using Cool.Normalization;

namespace normalizationtests.Web.Startup
{
    [DependsOn(typeof(NormalizationModule))]
    public class normalizationtestsWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public normalizationtestsWebModule(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            normalizationtestsLocalizationConfigurer.Configure( Configuration.Localization );

            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(normalizationtestsConsts.ConnectionStringName);

            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof( normalizationtestsWebModule ).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( normalizationtestsWebModule ).GetAssembly() );
            IocManager.RegisterAllClient( typeof( normalizationtestsWebModule ).GetAssembly() );
        }
    }
}