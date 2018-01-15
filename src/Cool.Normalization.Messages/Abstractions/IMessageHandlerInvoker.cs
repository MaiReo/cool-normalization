using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages
{
    public interface IMessageHandlerInvoker
    {
        Task InvokeAsync(IReadOnlyDictionary<string, Type> messages, IMessageWrapper wrapper);
    }
}
