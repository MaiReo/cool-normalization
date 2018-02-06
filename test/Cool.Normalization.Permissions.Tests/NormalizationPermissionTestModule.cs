using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Reflection.Extensions;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Permissions;
using System;

namespace Abp.Modules
{
    [DependsOn(
        typeof( NormalizationPermissionModule ),
        typeof( Abp.TestBase.AbpTestBaseModule )
        )]
    public class NormalizationPermissionTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IPrincipalAccessor,FakePrincipalAccessor>();
        }

        public override void Initialize()
        {
            
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationPermissionTestModule ).GetAssembly() );
        }
        
        private void RegisterIfNot<TService, TImpl>( DependencyLifeStyle lifeStyle = default ) where TService : class where TImpl : class, TService
        {
            if (IocManager.IsRegistered<TService>())
            {
                return;
            }
            IocManager.Register<TService, TImpl>( lifeStyle );
        }

        private void RegisterIfNot<T>( Abp.Dependency.DependencyLifeStyle lifeStyle = default ) where T : class
        {
            if (IocManager.IsRegistered<T>())
            {
                return;
            }
            IocManager.Register<T>( lifeStyle );
        }
    }
}