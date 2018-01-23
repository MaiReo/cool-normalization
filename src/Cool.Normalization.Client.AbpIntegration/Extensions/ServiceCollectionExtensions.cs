#if NETSTANDARD2_0
using Cool.Normalization;
using Cool.Normalization.Client;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Try Add <see cref="GuidToStringRequestIdGenerator"/> as <see cref="IRequestIdGenerator"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection TryAddRequestIdGenerator(this IServiceCollection services)
        {
            services.TryAddScoped<IRequestIdGenerator, GuidToStringRequestIdGenerator>();
            return services;
        }
        /// <summary>
        /// Try add <see cref="PassthroughRequestIdSetter"/> as <see cref="IRequestIdSetter"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection TryAddPassthroughRequestIdSetter(this IServiceCollection services)
        {
            services.TryAddTransient<IRequestIdSetter, PassthroughRequestIdSetter>();
            return services;
        }

        /// <summary>
        /// Try add a <see cref="IRequestIdSetter"/> Depends on <see cref="IRequestIdGenerator"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection TryAddRequestIdSetter(this IServiceCollection services)
        {
            TryAddRequestIdGenerator( services );
            services.TryAddTransient<IRequestIdSetter, DefaultRequestIdSetter>();
            return services;
        }
    }
}
#endif