using MaiReo.Messages.Abstractions;
using System;
using System.Reflection;

namespace Cool.Normalization.Messages
{
    public interface IMessageLogFormatter
    {
        string Format(IMessageWrapper wrapper, 
            long elapsedMilliseconds,
            MethodInfo ifaceMethod = default( MethodInfo ),
            MethodInfo handlerMethod = default( MethodInfo ),
            Exception exception = default( Exception ));
    }
}
