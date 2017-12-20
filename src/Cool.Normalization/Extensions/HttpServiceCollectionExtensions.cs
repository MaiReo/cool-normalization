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
