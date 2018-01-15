using Cool.Normalization.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Extensions;
using System.Text;

namespace Cool.Normalization
{
    [AttributeUsage( AttributeTargets.Interface
        | AttributeTargets.Class
        | AttributeTargets.Method
        | AttributeTargets.Property,
        Inherited = true,
        AllowMultiple = true )]
    public class CodeAttribute : Attribute, ICodeAttribute
    {
        public CodeAttribute(string code)
        {
            this.Code = code;
            CodePart = CodePart.Default;
        }

        public virtual CodePart CodePart { get; set; }

        public virtual string Code { get; set; }

        /// <summary>
        /// generate success code.
        /// </summary>
        /// <param name="onMethod"></param>
        /// <param name="onClass"></param>
        /// <param name="onInterfaceMethod"></param>
        /// <param name="onInterface"></param>
        /// <param name="levelFallback"></param>
        /// <param name="detailFallback"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<CodePart, string> GenerateCodesForSuccess(
            IEnumerable<ICodeAttribute> onMethod,
            IEnumerable<ICodeAttribute> onClass,
            IEnumerable<ICodeAttribute> onInterfaceMethod,
            IEnumerable<ICodeAttribute> onInterface,
            string levelFallback,
            ICodeAttribute detailFallback = default( ICodeAttribute ))
        {
            // Ordering found codes :  Class Method > Class > Interface Method > Interface > Fallback > Default
            var allCodes = onMethod
                .ConcatSkipNullOrEmpty( onClass )
                .ConcatSkipNullOrEmpty( onInterfaceMethod )
                .ConcatSkipNullOrEmpty( onInterface )
                ?.ToList() ?? new List<ICodeAttribute>( 0 );

            var levelCode = allCodes.GetPartCodeOrFallbackOrDefault( CodePart.Level,
                    null, levelFallback );

            var serviceCode = allCodes.GetPartCodeOrFallbackOrDefault( CodePart.Service,
                    onClass
                    .ConcatSkipNullOrEmpty( onInterface )
                    ?.Where( c => (c?.CodePart & CodePart.Default) == CodePart.Default )
                    ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) ), Codes.Service.Default );

            var apiCode = allCodes.GetPartCodeOrFallbackOrDefault( CodePart.Api,
                    onMethod
                    .ConcatSkipNullOrEmpty( onInterfaceMethod )
                    ?.Where( c => (c?.CodePart & CodePart.Default) == CodePart.Default )
                    ?.FirstOrDefault( c => !string.IsNullOrWhiteSpace( c?.Code ) ), Codes.Api.Default );

            var detailCode = allCodes.GetPartCodeOrFallbackOrDefault( CodePart.Detail,
                      detailFallback, Codes.Detail.Default );

            return new Dictionary<CodePart, string>
            {
                { CodePart.Level,levelCode },
                { CodePart.Service,serviceCode },
                { CodePart.Api,apiCode },
                { CodePart.Detail,detailCode },
            };

        }

        public static IReadOnlyDictionary<CodePart, string> GenerateCodesForError(
            IReadOnlyDictionary<CodePart, string> successCodes,
            Exception exception)
        {
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
                { CodePart.Service,successCodes[CodePart.Service] },
                { CodePart.Api,successCodes[CodePart.Api] },
                { CodePart.Detail,detailCode ?? Codes.Detail.Default },
            };
        }
    }
}
