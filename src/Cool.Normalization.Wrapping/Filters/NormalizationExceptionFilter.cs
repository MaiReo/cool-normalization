using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.Extensions;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Logging;
using Castle.Core.Logging;
using Cool.Normalization.Configuration;
using Cool.Normalization.Wrapping;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Reflection;

namespace Cool.Normalization.Filters
{
    public class NormalizationExceptionFilter : IExceptionFilter, IFilterMetadata
    {
        public ILogger Logger { get; set; }

        public IEventBus EventBus { get; set; }

        public static TypeInfo AbpReflectionHelperType { get; }

        public INormalizationExceptionWrapperFactory NormalizationExceptionWrapperFactory { get; set; }

        private readonly IAbpAspNetCoreConfiguration _configuration;



        public NormalizationExceptionFilter(IAbpAspNetCoreConfiguration configuration,
            INormalizationConfiguration normalizationConfiguration)
        {

            _configuration = configuration;

            Logger = NullLogger.Instance;
            EventBus = NullEventBus.Instance;
        }


        private static TAttribute GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(MemberInfo memberInfo, TAttribute defaultAttribute) where TAttribute : Attribute
        {
            return memberInfo.GetCustomAttributes( true ).OfType<TAttribute>().FirstOrDefault()
                    ?? memberInfo.DeclaringType?.GetTypeInfo().GetCustomAttributes( true ).OfType<TAttribute>().FirstOrDefault()
                    ?? defaultAttribute;
        }

        public void OnException(ExceptionContext context)
        {

            if (!context.ActionDescriptor.IsControllerAction())
            {
                return;
            }

            var wrapResultAttribute = GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
                    context.ActionDescriptor.GetMethodInfo(),
                    _configuration.DefaultWrapResultAttribute
                );

            if (wrapResultAttribute.LogError)
            {
                LogHelper.LogException( Logger, context.Exception );
            }

            if (wrapResultAttribute.WrapOnError)
            {
                NormalizationExceptionWrapperFactory.CreateFor( context ).Wrap( context );
            }


        }
    }
}
