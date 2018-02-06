using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Reflection.Extensions;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Permissions;
using System;

using cool.permission.client.Api;
using cool.permission.client.Model;

namespace Abp.Modules
{
    [DependsOn(
        typeof( NormalizationPermissionRemoteProxyModule ),
        typeof( Abp.TestBase.AbpTestBaseModule )
        )]
    public class NormalizationPermissionRemoteProxyTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.ReplaceService<IPermissionApi, FakePermissionApi>();
        }

        public override void Initialize()
        {
            
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationPermissionRemoteProxyTestModule ).GetAssembly() );
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