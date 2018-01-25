#if NETSTANDARD2_0
using Cool.Normalization;
using Cool.Normalization.Client;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Cool.Normalization.Utilities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RequestIdServiceCollectionExtensions
    {
        /// <summary>
        /// 尝试添加使用Guid实现的<see cref="GuidToStringRequestIdGenerator"/>
        /// 作为<see cref="IRequestIdGenerator"/>到依赖注入容器。
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection TryAddRequestIdGenerator(
            this IServiceCollection services)
        {
            services.TryAddScoped<IRequestIdGenerator,
                GuidToStringRequestIdGenerator>();
            return services;
        }
        /// <summary>
        /// 尝试添加透传从<see cref="IRequestIdAccessor"/>获取到的
        /// <see cref="IRequestIdAccessor.RequestId"/>
        /// 实现类<see cref="PassthroughRequestIdSetter"/>
        /// 作为<see cref="IRequestIdSetter"/>到依赖注入容器。
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection TryAddPassthroughRequestIdSetter(
            this IServiceCollection services)
        {
            services.TryAddTransient<IRequestIdSetter,
                PassthroughRequestIdSetter>();
            return services;
        }

        /// <summary>
        /// 尝试添加透传从<see cref="IRequestIdGenerator"/>获取到的
        /// <see cref="IRequestIdGenerator.RequestId"/>
        /// 实现类<see cref="DefaultRequestIdSetter"/>
        /// 作为<see cref="IRequestIdSetter"/>到依赖注入容器。
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <returns><paramref name="services"/></returns>
        public static IServiceCollection TryAddRequestIdSetter(
            this IServiceCollection services)
        {
            TryAddRequestIdGenerator( services );
            services.TryAddTransient<IRequestIdSetter,
                DefaultRequestIdSetter>();
            return services;
        }
    }
}
#endif