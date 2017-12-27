using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages
{
    public interface IMessageLogFormatter
    {
        string Format( IMessageWrapper wrapper, long elapsedMilliseconds, Type messageType, Type handlerType = null, Exception exception = null );
    }
}
