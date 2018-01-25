using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Extensions;
using System.Reflection;
using Abp.Dependency;
using Cool.Normalization.Utilities;
using MaiReo.Messages.Abstractions;

namespace Cool.Normalization.Messages
{
    public class MessageLogFormatter : IMessageLogFormatter,ISingletonDependency
    {
        public const string PREFIX = "MESSAGE-HANDLED";

        public IMessageHandlerCodeResolver CodeResolver { get; set; }

        public string Format(IMessageWrapper wrapper,
            long elapsedMilliseconds,
            MethodInfo ifaceMethod = default( MethodInfo ),
            MethodInfo implMethod = default( MethodInfo ),
            Exception exception = default( Exception ))
        {
            var codes = CodeResolver.ResolveCode( ifaceMethod, implMethod, exception );
            var code = SetMissingCodes( codes ).CombineCodes();

            return JoinWith( "^", default( string ),
                PREFIX, $"{elapsedMilliseconds}",
                code, implMethod?.DeclaringType?.FullName,
                wrapper?.Topic,
                (wrapper?.Timestamp)?.ToString(),
                wrapper?.Message,
                exception?.Message );
        }

        private static string JoinWith(string sep, string defaultValue, params string[] items)
        {
            return string.Join( sep, items?.Select( item => item ?? defaultValue ?? "NULL" ) );
        }

        private static IReadOnlyDictionary<CodePart, string> SetMissingCodes(
            IReadOnlyDictionary<CodePart, string> codes)
        {
            var defaultCodes = new Dictionary<CodePart, string>()
            {
                {  CodePart.Level, Codes.Level.Success },
                {  CodePart.Service, Codes.Service.Default },
                {  CodePart.Api, Codes.Api.Default },
                {  CodePart.Detail, Codes.Detail.Default }
            };
            if (codes == null)
                return defaultCodes;
            foreach (var key in codes.Keys)
            {
                if (defaultCodes.ContainsKey( key ))
                {
                    defaultCodes[key] = codes[key] ?? defaultCodes[key];
                }
            }
            return defaultCodes;
        }
       
    }
}