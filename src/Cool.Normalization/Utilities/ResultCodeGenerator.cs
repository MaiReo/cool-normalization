using Abp.Dependency;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Linq;
using Abp.AspNetCore.Mvc.Extensions;
using Abp.Runtime.Validation;
using System.Linq.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Abp.Authorization;

namespace Cool.Normalization.Utilities
{
    internal class ResultCodeGenerator : IResultCodeGenerator, ISingletonDependency
    {

        public string GetCode(ActionDescriptor actionDescriptor, Type controllerType)
            => GetCodes( actionDescriptor ).CombineCodes();

        public string GetCode(ExceptionContext exceptionContext, Type controllerType)
            => GetCodes( exceptionContext, controllerType ).CombineCodes();

        private static IReadOnlyDictionary<CodePart, string> GetCodes(ExceptionContext exceptionContext,
            Type controllerRealType)
        {
            var actionDescriptor = exceptionContext?.ActionDescriptor;
            var controllerAction = actionDescriptor?.AsControllerActionDescriptor();
            var exception = exceptionContext?.Exception;

            var codes = GetCodes( actionDescriptor, Codes.Level.Fatal );
            if (exception is AbpValidationException ave)
            {
                var newCodes = codes.ToDictionary( c => c.Key, c => c.Value );
                newCodes[CodePart.Level] = Codes.Level.ArgumentError;
                return GetAbpValidationExceptionCodes( newCodes, controllerAction?.Parameters, ave );
            }
            return CodeAttribute.GenerateCodesForError( codes, exception );

        }



        private static IReadOnlyDictionary<CodePart, string>
        GetCodes(ActionDescriptor actionDescriptor,
            string defaultLevelCode = Codes.Level.Success,
            Type controllerRealType = null)
        {
            if (string.IsNullOrWhiteSpace( defaultLevelCode )) throw new ArgumentException( nameof( defaultLevelCode ) );
            var controllerDescriptor = actionDescriptor?.AsControllerActionDescriptor();
            var controllerType = controllerDescriptor?.ControllerTypeInfo;
            var interfaceType = controllerType.ImplementedInterfaces?.FirstOrDefault( i => i.IsPublic && i.Name == "I" + controllerType.Name );
            var actionMethod = controllerDescriptor?.MethodInfo?.GetBaseDefinition();
            var parameterTypes = actionMethod?.GetParameters()?.Select( p => p.ParameterType )?.ToArray() ?? new Type[0];
            var interfaceMethod = interfaceType?.GetMethod( actionMethod?.Name, parameterTypes );

            var actionCodes = actionMethod?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();
            var interfaceMethodCodes = interfaceMethod?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();
            var controllerCodes = controllerType?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();
            var interfaceCodes = interfaceType?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();

            var returnType = actionMethod?.ReturnType;
            if (returnType == null || returnType == typeof( void ))
            {
                returnType = null;
            }
            else
            {
                if (typeof( Task ).IsAssignableFrom( returnType )
                    && returnType.IsGenericType)
                {
                    returnType = returnType.GetGenericArguments().FirstOrDefault();
                }
            }

            var outputDtoDetailCode = returnType
                ?.GetCustomAttributes( true )
                ?.OfType<ICodeAttribute>()
                ?.Where( c => (c?.CodePart & CodePart.Default) == CodePart.Default
                    || (c?.CodePart & CodePart.Detail) == CodePart.Detail )
                ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) );

            // Ordering finded codes :  Action Method > Controller > Interface Method > Interface > Fallback > Default

            return CodeAttribute.GenerateCodesForSuccess( actionCodes,
                controllerCodes,
                interfaceMethodCodes,
                interfaceCodes,
                defaultLevelCode,
                outputDtoDetailCode );
        }

        private static IReadOnlyDictionary<CodePart, string>
        GetAbpValidationExceptionCodes(
            IReadOnlyDictionary<CodePart, string> codes,
            IList<ParameterDescriptor> parameters,
            AbpValidationException abpValidationException)
        {
            var firstPropertyName = abpValidationException?.ValidationErrors
                        ?.FirstOrDefault()?.MemberNames
                        ?.FirstOrDefault() ?? "_";
            var firstMatchedParameterType = parameters
                ?.FirstOrDefault( p => p?.ParameterType?.IsPrimitive == false
                && p?.ParameterType?.GetProperty( firstPropertyName )
                    ?.GetCustomAttributes( true )
                    ?.OfType<ValidationAttribute>()
                    ?.Any() == true )
                ?.ParameterType;
            var firstMatchedProperty = firstMatchedParameterType?.GetProperty( firstPropertyName );

            return GetCodesFromProperty( codes, firstMatchedParameterType, firstMatchedProperty );
        }

        private static IReadOnlyDictionary<CodePart, string>
        GetCodesFromProperty(IReadOnlyDictionary<CodePart, string> codes,
            Type declaringType, PropertyInfo property)
        {

            //Order rule: Property > Declaring Type > Default

            var propCodes = property?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();
            var declaringTypeCodes = (declaringType ?? property?.DeclaringType)
                    ?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();

            var allCodes = propCodes.ConcatSkipNullOrEmpty( declaringTypeCodes )
                    ?.ToList() ?? new List<ICodeAttribute>( 0 );

            var levelCode = allCodes.GetPartCodeOrFallbackOrDefault( CodePart.Level,
                    declaringTypeCodes
                    ?.Where( c => c?.CodePart == CodePart.Default )
                    ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) )
                    , Codes.Level.Fatal );

            var detailCode = allCodes.GetPartCodeOrFallbackOrDefault( CodePart.Level,
                    propCodes
                    ?.Where( c => c?.CodePart == CodePart.Default )
                    ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) )
                    , Codes.Detail.Default );

            return new Dictionary<CodePart, string>
            {
                { CodePart.Level,levelCode },
                { CodePart.Service,codes[CodePart.Service] },
                { CodePart.Api,codes[CodePart.Api] },
                { CodePart.Detail,detailCode },
            };
        }

    }
}
