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

namespace Cool.Normalization.Utilities
{
    internal class ResultCodeGenerator : IResultCodeGenerator, ISingletonDependency
    {

        public string GetCode( ActionDescriptor actionDescriptor, Type controllerType )
            => CombineCodes( GetCodes( actionDescriptor ) );

        public string GetCode( ExceptionContext exceptionContext, Type controllerType )
            => CombineCodes(
                GetCodes( exceptionContext, controllerType ) );

        private static IDictionary<CodePart, string> GetCodes( ExceptionContext exceptionContext,
            Type controllerRealType )
        {
            var actionDescriptor = exceptionContext?.ActionDescriptor;
            var controllerAction = actionDescriptor?.AsControllerActionDescriptor();
            var exception = exceptionContext?.Exception;

            var codes = GetCodes( actionDescriptor, Codes.Level.Fatal );
            if (exception is AbpValidationException ave)
            {
                return GetAbpValidationExceptionCodes( codes, controllerAction?.Parameters, ave );
            }


            var levelCode = Codes.Level.Fatal;
            var detailCode = Codes.Detail.Default;
            if (exception is NormalizationException ne)
            {
                levelCode = ne?.LevelCode;
                detailCode = ne?.DetailCode;
            }
            
            var exceptionType = exception?.GetType();
            var exceptionCodes = exceptionType?.GetCustomAttributes( true ).OfType<ICodeAttribute>();

            if (string.IsNullOrWhiteSpace( levelCode ) || levelCode == Codes.Level.Fatal)
            {
                levelCode = exceptionCodes?.FirstOrDefault( c =>
                (c?.CodePart & CodePart.Level) == CodePart.Level
                && !string.IsNullOrWhiteSpace( c?.Code ) )?.Code;
            }
            if (string.IsNullOrWhiteSpace( detailCode ) || detailCode == Codes.Detail.Default)
            {
                detailCode = exceptionCodes?.FirstOrDefault( c =>
               ((c?.CodePart & CodePart.Level) == CodePart.Level
                   || (c?.CodePart & CodePart.Default) == CodePart.Default)
               && !string.IsNullOrWhiteSpace( c?.Code ) )?.Code;
            }

            return new Dictionary<CodePart, string>
            {
                { CodePart.Level,levelCode ?? Codes.Level.Fatal },
                { CodePart.Service,codes[CodePart.Service] },
                { CodePart.Api,codes[CodePart.Api] },
                { CodePart.Detail,detailCode ?? Codes.Detail.Default },
            };
        }

        

        private static IDictionary<CodePart, string>
        GetCodes( ActionDescriptor actionDescriptor,
            string defaultLevelCode = Codes.Level.Success,
            Type controllerRealType = null )
        {
            if (string.IsNullOrWhiteSpace( defaultLevelCode )) throw new ArgumentException( nameof( defaultLevelCode ) );
            var controllerDescriptor = actionDescriptor?.AsControllerActionDescriptor();
            var controllerType = controllerDescriptor?.ControllerTypeInfo;
            var interfaceType = controllerType.ImplementedInterfaces?.FirstOrDefault( i => i.IsPublic && i.Name == "I" + controllerType.Name );
            var actionMethod = controllerDescriptor?.GetMethodInfo();
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
            var allCodes = actionCodes
                .ConcatSkipNullOrEmpty( controllerCodes )
                .ConcatSkipNullOrEmpty( interfaceMethodCodes )
                .ConcatSkipNullOrEmpty( interfaceCodes )
                ?.ToList() ?? new List<ICodeAttribute>( 0 );


            var levelCode = GetPartCodeOrFallbackOrDefault( allCodes, CodePart.Level,
                    null, defaultLevelCode );

            var serviceCode = GetPartCodeOrFallbackOrDefault( allCodes, CodePart.Service,
                    controllerCodes
                    .ConcatSkipNullOrEmpty( interfaceCodes )
                    ?.Where( c => (c?.CodePart & CodePart.Default) == CodePart.Default )
                    ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) ), Codes.Service.Default );

            var apiCode = GetPartCodeOrFallbackOrDefault( allCodes, CodePart.Api,
                    actionCodes
                    .ConcatSkipNullOrEmpty( interfaceMethodCodes )
                    ?.Where( c => (c?.CodePart & CodePart.Default) == CodePart.Default )
                    ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) ), Codes.Api.Default );

            var detailCode = GetPartCodeOrFallbackOrDefault( allCodes, CodePart.Detail,
                      outputDtoDetailCode, Codes.Detail.Default );

            return new Dictionary<CodePart, string>
            {
                { CodePart.Level,levelCode },
                { CodePart.Service,serviceCode },
                { CodePart.Api,apiCode },
                { CodePart.Detail,detailCode },
            };
        }

        private static IDictionary<CodePart, string>
        GetAbpValidationExceptionCodes(
            IDictionary<CodePart, string> codes,
            IList<ParameterDescriptor> parameters,
            AbpValidationException abpValidationException )
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

        private static IDictionary<CodePart, string>
        GetCodesFromProperty( IDictionary<CodePart, string> codes,
            Type declaringType, PropertyInfo property )
        {//

            //Order rule: Property > Declaring Type > Default

            var propCodes = property?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();
            var declaringTypeCodes = (declaringType ?? property?.DeclaringType)
                    ?.GetCustomAttributes( true )?.OfType<ICodeAttribute>();

            var allCodes = propCodes.ConcatSkipNullOrEmpty( declaringTypeCodes )
                    ?.ToList() ?? new List<ICodeAttribute>( 0 );

            var levelCode = GetPartCodeOrFallbackOrDefault( allCodes, CodePart.Level,
                    declaringTypeCodes
                    ?.Where( c => c?.CodePart == CodePart.Default )
                    ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) )
                    , Codes.Level.Fatal );

            var detailCode = GetPartCodeOrFallbackOrDefault( allCodes, CodePart.Level,
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

        private static string GetPartCodeOrFallbackOrDefault(
            IEnumerable<ICodeAttribute> codes, CodePart part,
            ICodeAttribute fallbackCode, string defaultCode )
            => (codes?.Where( c => c?.CodePart == part )
                ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) )
                ?? fallbackCode)?.Code ?? defaultCode;
        private static string CombineCodes( IDictionary<CodePart, string> codes )
           => string.Concat( codes.OrderBy( c => c.Key ).Select( c => c.Value ) );

    }
}
