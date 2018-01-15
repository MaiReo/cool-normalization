using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cool.Normalization.Messages
{
    public interface IMessageHandlerCodeResolver
    {
        IReadOnlyDictionary<CodePart, string> ResolveCode(
            MethodInfo ifaceMethod,
            MethodInfo implMethod,
            Exception exception = default( Exception ));
    }
}
