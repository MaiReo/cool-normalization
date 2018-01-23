#region 程序集 Version=1.0.6
/*
 * 模块初始化方法，用于MVC项目Configuration启动时调用。
 */
#endregion

using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.Mvc.ExceptionHandling;
using Cool.Normalization;
using Cool.Normalization.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NormalizationWrappingServiceCollectionExtensions
    {
        public static IServiceCollection AddNormalizationWrapping(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.TryAddScoped<IRequestIdGenerator, RequestIdGenerator>();

            services.PostConfigure<MvcOptions>( options =>
                options.ReplaceServiceFilter<AbpExceptionFilter, NormalizationExceptionFilter>()
                .ReplaceServiceFilter<AbpAuthorizationFilter, NormalizationAuthorizationFilter>()
             );
            return services;
        }

        private static MvcOptions ReplaceServiceFilter<TOriginal, TReplacement>(this MvcOptions options)
            where TOriginal : IFilterMetadata
            where TReplacement : IFilterMetadata
        {

            var originals = options.Filters.OfType<ServiceFilterAttribute>()
                .Where( filter => typeof( TOriginal ).IsAssignableFrom( filter.ServiceType ) )
                .ToList();
            foreach (var original in originals)
            {
                options.Filters.Remove( original );
            }
            options.Filters.AddService<TReplacement>();
            return options;
        }
    }
}
