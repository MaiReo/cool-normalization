#region 程序集 Version=1.0.6
/*
 * 扩展方法
 * HttpContext.Current
 */
#endregion

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Integrated in AspNet Core 2.1 or above.
    /// </summary>
    internal static class HttpServiceCollectionExtensions
    {
        internal static IServiceCollection AddHttpContextAccessor( this IServiceCollection services )
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
    }
}
