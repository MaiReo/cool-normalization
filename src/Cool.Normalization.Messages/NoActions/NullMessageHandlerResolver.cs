using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages
{
    public class NullMessageHandlerResolver : IMessageHandlerResolver
    {
        public object Resolve(IIocResolver iocResolver, Type messageType)
        => null;

        public static NullMessageHandlerResolver Instance 
            => new NullMessageHandlerResolver();
    }
}
