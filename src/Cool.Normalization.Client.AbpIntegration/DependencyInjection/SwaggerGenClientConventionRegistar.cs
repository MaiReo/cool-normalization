using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Cool.Normalization.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Client
{
    internal class SwaggerGenClientConventionRegistar : IConventionalDependencyRegistrar
    {
        public void RegisterAssembly(IConventionalRegistrationContext context)
        {
            context.IocManager.IocContainer.Register(
                Classes.FromAssembly( context.Assembly )
                .RegisterClient()
            );
        }

        
    }
}
