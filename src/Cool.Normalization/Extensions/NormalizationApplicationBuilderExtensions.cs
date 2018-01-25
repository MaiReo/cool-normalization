using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class NormalizationApplicationBuilderExtensions
    {
        /// <summary>
        /// 配置使用Cool.Normalization框架
        /// </summary>
        /// <param name="app"></param>
        /// <param name="useAuth">使用验证(默认不使用)</param>
        /// <param name="useSwagger">添加swagger(默认)</param>
        /// <returns></returns>
        public static IApplicationBuilder UseNormalization(
            this IApplicationBuilder app,
            bool? useAuth = default,
            bool? useSwagger = default)
        {
            if (useAuth == true)
            {
                app.UseAuth();
            }
            if (useSwagger != false)
            {
                app.UseSwaggerEtc(
                    NormalizationServiceCollectionExtensions.FriendlyName );
            }
            return app;
        }
    }
}
