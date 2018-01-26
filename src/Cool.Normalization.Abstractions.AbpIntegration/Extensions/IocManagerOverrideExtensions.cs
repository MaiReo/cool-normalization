using Abp.Dependency;
using Castle.MicroKernel.Registration;

namespace Abp.Modules
{
    public static class IocManagerOverrideExtensions
    {
        /// <summary>
        /// 设置<typeparamref ref="TImpl"/>为
        /// <typeparamref name="TService"/>的默认值。
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <typeparam name="TImpl">实现</typeparam>
        /// <param name="iocManager">依赖注入管理器</param>
        /// <param name="lifeStyle">生存周期</param>
        /// <returns></returns>
        public static IIocManager ReplaceService<TService, TImpl>(
            this IIocManager iocManager,
            DependencyLifeStyle lifeStyle = default( DependencyLifeStyle ))
            where TImpl : class, TService
            where TService : class
        {
            var registration = Component
                .For<TService, TImpl>()
                .ImplementedBy<TImpl>()
                .IsDefault();

            switch (lifeStyle)
            {
                case DependencyLifeStyle.Transient:
                    registration = registration.LifestyleTransient();
                    break;
                case DependencyLifeStyle.Singleton:
                default:
                    registration = registration.LifestyleSingleton();
                    break;
            }

            iocManager.IocContainer.Register( registration );
            return iocManager;
        }
    }
}
