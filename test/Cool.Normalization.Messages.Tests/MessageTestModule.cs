using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Abp.EntityFrameworkCore;

namespace Cool.Normalization.Messages.Tests
{
    [DependsOn(
        typeof( NormalizationMessageModule ),
        typeof( Abp.EntityFrameworkCore.AbpEntityFrameworkCoreModule ),
        typeof( Abp.TestBase.AbpTestBaseModule )
        )]
    public class MessageTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
            Configuration.Modules.Normalization().IsMessageEnabled = true;
            SetupInMemoryDb();
        }

        public override void Initialize()
        {
            Configuration.Modules.MessagePublisher().AutoStart = false;
            IocManager.RegisterAssemblyByConvention( typeof( MessageTestModule ).GetAssembly() );
        }

        private void SetupInMemoryDb()
        {
            var services = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(
                IocManager.IocContainer,
                services
            );
            var databaseName = System.Guid.NewGuid().ToString( "N" );

            services.AddAbpDbContext<TestDbContext>( options =>
                options.DbContextOptions
                    .UseInMemoryDatabase( databaseName )
                    .UseInternalServiceProvider( serviceProvider ) );

            var builder = new DbContextOptionsBuilder<TestDbContext>();
            builder.UseInMemoryDatabase( databaseName )
                .UseInternalServiceProvider( serviceProvider );

            if (!IocManager.IsRegistered<DbContextOptions<TestDbContext>>())
            {
                IocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<TestDbContext>>()
                    .Instance( builder.Options )
                    .LifestyleSingleton()
                );
            }

        }
    }
}