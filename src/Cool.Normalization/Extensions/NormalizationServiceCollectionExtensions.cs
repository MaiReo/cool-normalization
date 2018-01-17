using Abp.AspNetCore.Mvc.ExceptionHandling;
using Cool.Normalization;
using Cool.Normalization.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NormalizationServiceCollectionExtensions
    {
        public static IServiceCollection AddNormalization( this IServiceCollection services )
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.TryAddScoped<IRequestIdGenerator, RequestIdGenerator>();
            
            services.PostConfigure<MvcOptions>( options =>
            {
                var abpExceptionFilter = options.Filters.OfType<ServiceFilterAttribute>().FirstOrDefault( filter => filter.ServiceType == typeof( AbpExceptionFilter ) );
                if (abpExceptionFilter != null)
                {
                    options.Filters.Remove( abpExceptionFilter );
                }
                options.Filters.AddService<NormalizationExceptionFilter>();
            } );
            return services;
        }
    }
}
