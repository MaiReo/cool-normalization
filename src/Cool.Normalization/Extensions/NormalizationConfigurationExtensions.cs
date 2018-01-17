#region 程序集 Version=1.0.6
/*
 * 扩展方法
 * Abp推荐的获取配置的优雅写法
 * void Initialize()
 * {
 *   Configuration.Modules.Normalization().RequestIdHeaderName="X-Cool-RequestId";
 * }
 */
#endregion

using Abp.Configuration.Startup;
using Cool.Normalization.Configuration;

namespace Cool.Normalization
{
    public static class NormalizationConfigurationExtensions
    {
        public static INormalizationConfiguration Normalization( this IModuleConfigurations module )
            => module.AbpConfiguration.Get<INormalizationConfiguration>();
    }
}
