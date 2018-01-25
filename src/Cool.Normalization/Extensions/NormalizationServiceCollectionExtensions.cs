using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NormalizationServiceCollectionExtensions
    {
        /// <summary>
        /// 友好名字
        /// </summary>
        public static string FriendlyName { get; private set; }
        static NormalizationServiceCollectionExtensions()
        {
            FriendlyName = Assembly.GetEntryAssembly().GetName().Name;
        }
        /// <summary>
        /// 添加Cool.Normalization到依赖注入容器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="friendlyName">友好名称。默认使用程序集的产品名称</param>
        /// <param name="addWrapping">添加输出包装(默认)</param>
        /// <param name="addSwagger">添加Swagger文档(默认)</param>
        /// <param name="addAuth">添加身份验证(不添加)</param>
        /// <returns></returns>
        public static IServiceCollection AddNormalization(
            this IServiceCollection services,
            string friendlyName = default,
            bool? addWrapping = default,
            bool? addSwagger = default,
            bool? addAuth = default)
        {
            if (string.IsNullOrWhiteSpace( friendlyName ))
            {
                friendlyName = FriendlyName;
            }
            else
            {
                FriendlyName = friendlyName;
            }
            if (addWrapping != false)
            {
                services.AddNormalizationWrapping();
            }
            if (addAuth == true)
            {
                services.AddAuth();
            }
            services.AddSwaggerEtc( friendlyName, addAuth == true );
            return services;
        }
    }
}
