using Cool.Normalization.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cool.Normalization.Messages
{
    public class NullMessageHandlerCodeResolver : IMessageHandlerCodeResolver
    {
        public IReadOnlyDictionary<CodePart, string> ResolveCode(
            MethodInfo ifaceMethod,
            MethodInfo implMethod,
            Exception exception = default( Exception )) =>
            new Dictionary<CodePart, string>
            {
                { CodePart.Level, exception == default(Exception)
                        ?  Codes.Level.Success : Codes.Level.Fatal },
                { CodePart.Service, Codes.Service.Default },
                { CodePart.Api, Codes.Api.Default },
                { CodePart.Detail, Codes.Detail.Default }
            };

        public static NullMessageHandlerCodeResolver Instance
            => new NullMessageHandlerCodeResolver();
    }
}
