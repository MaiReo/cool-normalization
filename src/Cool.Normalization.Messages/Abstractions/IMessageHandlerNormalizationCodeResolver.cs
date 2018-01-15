using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cool.Normalization.Messages.Abstractions
{
    public interface IMessageHandlerNormalizationCodeResolver
    {
        Dictionary<CodePart, string> Resolve(Type handlerType, MethodInfo method, Exception exception = null);
    }
}
