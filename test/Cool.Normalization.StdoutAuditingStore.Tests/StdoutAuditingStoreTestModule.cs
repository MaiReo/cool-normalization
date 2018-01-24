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
using Castle.Core.Logging;

namespace Cool.Normalization.Tests
{
    [DependsOn(
        typeof( NormalizationWrappingTestModule ),
        typeof( NormalizationStdoutAuditingStoreModule )
        )]
    public class StdoutAuditingStoreTestModule : AbpModule
    {



        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(StdoutAuditingStoreTestModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!IocManager.IsRegistered<ILogger>())
            {
                IocManager.Register<ILogger, ConsoleLogger>();
            }
        }

    }
}