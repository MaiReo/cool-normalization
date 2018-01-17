/*
 * void Initialize()
 * {
 *   Configuration.Modules.Normalization().RequestIdHeaderName="X-Cool-RequestId";
 * }
 */
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
