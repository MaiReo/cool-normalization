using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Cool.Normalization;
using Cool.Normalization.Client;
using System.Reflection;

namespace Abp.Modules
{
    public static class IocManagerExtensions
    {
        public static IIocManager RegisterAllClient(this IIocManager iocManager
#if !NET452
            , Assembly rootAssembly
#endif
            )
        {
            iocManager.IocContainer.Register(
                Classes.FromAssemblyInThisApplication(
#if !NET452
                    rootAssembly
#endif
                    )
                    .RegisterClient()
            );
            return iocManager;
        }

#if NET452
        /// <summary>
        /// 添加在Web请求期间唯一的服务。
        /// 注意:此功能可能依赖于Web.Config
        /// 详见 Castle.Windsor
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImpl"></typeparam>
        /// <param name="iocManager"></param>
        /// <returns></returns>
        public static IIocManager RegisterPerWebRequest<TService, TImpl>(
            this IIocManager iocManager)
            where TImpl : class, TService
            where TService : class
        {
            iocManager.IocContainer.Register(
                Component.For<TService, TImpl>()
                .ImplementedBy<TImpl>()
                .LifestylePerWebRequest()
                );
            return iocManager;
        }

        /// <summary>
        /// 注册<see cref="IRequestIdGenerator"/>到依赖注入
        /// </summary>
        /// <param name="iocManager"></param>
        /// <returns></returns>
        public static IIocManager RegisterRequestIdGenerator(
            this IIocManager iocManager) 
            => iocManager.RegisterPerWebRequest<IRequestIdGenerator,
                GuidToStringRequestIdGenerator>();
#endif

    }
}
