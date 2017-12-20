using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.Extensions;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Logging;
using Castle.Core.Logging;
using Cool.Normalization.Wrapping;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Cool.Normalization.Configuration;

namespace Cool.Normalization.Filters
{
    public class NormalizationExceptionFilter : IExceptionFilter, IFilterMetadata, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public IEventBus EventBus { get; set; }

        public static TypeInfo AbpReflectionHelperType { get; }

        public INormalizationExceptionWrapperFactory normalizationExceptionWrapperFactory { get; set; }

        private readonly IAbpAspNetCoreConfiguration _configuration;

        

        public NormalizationExceptionFilter(IAbpAspNetCoreConfiguration configuration,
            INormalizationConfiguration normalizationConfiguration)
        {

            _configuration = configuration;

            Logger = NullLogger.Instance;
            EventBus = NullEventBus.Instance;
        }


        private static TAttribute GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>( MemberInfo  memberInfo, TAttribute defaultAttribute ) where TAttribute : Attribute
        {
            return memberInfo.GetCustomAttributes( true ).OfType<TAttribute>().FirstOrDefault()
                    ?? memberInfo.DeclaringType?.GetTypeInfo().GetCustomAttributes( true ).OfType<TAttribute>().FirstOrDefault()
                    ?? defaultAttribute;
        }
        
        public void OnException( ExceptionContext context )
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
                normalizationExceptionWrapperFactory.CreateFor( context ).Wrap( context );
            }

            
        }
    }
}
