using Abp.Dependency;
using Castle.MicroKernel.Registration;
using System.Reflection;

namespace Abp.Modules
{
    public static class IocManagerExtensions
    {
        public static IIocManager RegisterAllClient(this IIocManager iocManager
#if !NET452
            ,Assembly rootAssembly
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


    }
}
