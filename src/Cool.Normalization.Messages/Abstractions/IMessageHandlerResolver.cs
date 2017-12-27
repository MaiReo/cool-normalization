using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages
{
    public interface IMessageHandlerResolver
    {
        object Resolve( IIocResolver iocResolver, Type messageType );
    }
}
