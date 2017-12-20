using System;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Cool.Normalization.Auditing;
using Microsoft.AspNetCore.Http;
using Castle.MicroKernel.Registration;

namespace Cool.Normalization.Tests
{
    [DependsOn(
        typeof( NormalizationModule ),
        typeof( Abp.TestBase.AbpTestBaseModule )
        )]
    public class MicroServicesnormalizationTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            RegisterIfNot<Microsoft.AspNetCore.Mvc.ApplicationParts.ApplicationPartManager>();
            RegisterIfNot<IResultAuditingStore, MemoryListResultAuditingStore>();
            RegisterIfNot<IHttpContextAccessor, TestHttpContextAccessor>();
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.

            IocManager.IocContainer.Register(
                Component
                .For<HttpContext>()
                .UsingFactoryMethod( () => TestHttpContext.Current )
                .LifestyleTransient()
            );

            SetupInMemoryDb();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( MicroServicesnormalizationTestModule ).GetAssembly() );
        }
        private void SetupInMemoryDb()
        {
            var services = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(
                IocManager.IocContainer,
                services
            );

            //var builder = new DbContextOptionsBuilder<>();
            //builder.UseInMemoryDatabase().UseInternalServiceProvider( serviceProvider );

            //IocManager.IocContainer.Register(
            //    Component
            //        .For<DbContextOptions<>>()
            //        .Instance( builder.Options )
            //        .LifestyleSingleton()
            //);
        }

        public override void PostInitialize()
        {
            RegisterIfNot<IHttpContextAccessor, HttpContextAccessor>();
            RegisterIfNot<IActionContextAccessor, ActionContextAccessor>();
        }

        private void RegisterIfNot<TService, TImpl>( Abp.Dependency.DependencyLifeStyle lifeStyle = Abp.Dependency.DependencyLifeStyle.Singleton ) where TService : class where TImpl : class, TService
        {
            if (IocManager.IsRegistered<TService>())
            {
                return;
            }
            IocManager.Register<TService, TImpl>( lifeStyle );
        }

        private void RegisterIfNot<T>( Abp.Dependency.DependencyLifeStyle lifeStyle = Abp.Dependency.DependencyLifeStyle.Singleton ) where T : class
        {
            if (IocManager.IsRegistered<T>())
            {
                return;
            }
            IocManager.Register<T>( lifeStyle );
        }
    }
}