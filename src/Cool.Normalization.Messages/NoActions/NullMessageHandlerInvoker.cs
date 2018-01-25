using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages
{
    public class NullMessageHandlerInvoker: IMessageHandlerInvoker
    {
        public Task InvokeAsync(IReadOnlyDictionary<string, Type> messages, IMessageWrapper wrapper)
            => Task.CompletedTask;

        public static NullMessageHandlerInvoker Instance
            => new NullMessageHandlerInvoker();
    }
}
