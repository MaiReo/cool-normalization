using Cool.Normalization.Client;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Castle.MicroKernel.Registration
{
    internal static class CastleRegistationDescriptorExtensions
    {
        public static IRegistration[] RegisterClient(
            this FromAssemblyDescriptor descriptor)
        {
            return new[]
            {
                descriptor.BasedOn<IApiAccessor>()
                    .WithServiceDefaultInterfaces()
                    .WithServiceSelf()
                    .LifestyleTransient()
                ,
                descriptor
                    .BasedOn<IReadableConfiguration>()
                    .Configure(
                        conf => conf.UsingFactoryMethod(
                            (kernel,model,context)
                                => model.Constructors
                                .First(ctor=>ctor.Dependencies.Length == 0)
                                .Constructor.Invoke(new object[0]))
                    .LifestyleTransient()
                    )
            };
        }
    }
}
