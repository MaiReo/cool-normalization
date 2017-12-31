using System;
using MaiReo.Messages.Abstractions;

namespace Cool.Normalization.Messages
{
    public class MessageLogFormatter : IMessageLogFormatter
    {
        public string Format( IMessageWrapper wrapper,
            long elapsedMilliseconds,
            Type messageType,
            Type handlerType = null,
            Exception exception = null )
        {
            return $"MESSAGE-HANDLED^{messageType.FullName}^{handlerType?.FullName ?? "NULL"}^{elapsedMilliseconds}^{wrapper?.Topic}^{(wrapper?.Timestamp)?.ToString() ?? "NULL"}^{wrapper?.Message}^{exception?.Message ?? "NULL"}";
        }
    }
}