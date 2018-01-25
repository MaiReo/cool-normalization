using MaiReo.Messages.Abstractions;
using System;
using System.Reflection;

namespace Cool.Normalization.Messages
{
    public class NullMessageLogFormatter : IMessageLogFormatter
    {
        public string Format(IMessageWrapper wrapper,
            long elapsedMilliseconds,
            MethodInfo ifaceMethod = default( MethodInfo ),
            MethodInfo handlerMethod = default( MethodInfo ),
            Exception exception = default( Exception ))
            => nameof( NullMessageLogFormatter );

        public static NullMessageLogFormatter Instance
            => new NullMessageLogFormatter();
    }
}
