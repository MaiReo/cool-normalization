using Abp.Dependency;
using MaiReo.Messages.Abstractions;
using System;

namespace Cool.Normalization.Messages
{
    public class MessageHandlerResolver : IMessageHandlerResolver, ISingletonDependency
    {
        public object Resolve( IIocResolver iocResolver, Type messageType )
        {
            var handlerType = typeof( IMessageHandler<> ).MakeGenericType( messageType );

            var handlerInstance = iocResolver.Resolve( handlerType );

            return handlerInstance;

        }
    }
}