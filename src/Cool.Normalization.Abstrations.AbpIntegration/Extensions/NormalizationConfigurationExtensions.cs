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

namespace Abp.Modules

{
    public static class NormalizationConfigurationExtensions
    {
        public static INormalizationConfiguration Normalization(
            this IModuleConfigurations module)
            => module.AbpConfiguration.Get<INormalizationConfiguration>(
#if NET452
                nameof( INormalizationConfiguration )
#endif
                );

        /// <summary>
        /// 获取标准输出审计日志配置
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static IStdoutAuditStoreConfiguration StdoutAuditingStore(
            this IModuleConfigurations module)
            => module.AbpConfiguration.Get<IStdoutAuditStoreConfiguration>(
#if NET452
                nameof( IStdoutAuditStoreConfiguration )
#endif
                );
    }
}
