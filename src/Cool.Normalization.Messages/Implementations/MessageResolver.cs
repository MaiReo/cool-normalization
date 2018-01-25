using Abp.Dependency;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cool.Normalization.Messages
{
    public class MessageResolver : IMessageResolver, ISingletonDependency
    {
        public IReadOnlyDictionary<string, Type> HasHandlerMessages(
            IIocResolver iocResolver )
        {
            var allMessageHandlers = iocResolver.ResolveAll<IMessageHandler>();
            var allMessages = new HashSet<Type>();
            if (allMessageHandlers != null)
            {
                foreach (var handler in allMessageHandlers)
                {

                    var messageHandlerIfaces = handler.GetType()
                    .GetInterfaces()
                    .Where( iface => iface.IsGenericType )
                    .Where( iface => iface.GetGenericTypeDefinition() == typeof( IMessageHandler<> ) );

                    var messages = messageHandlerIfaces.Select( iface => iface.GetGenericArguments().First() );
                    foreach (var message in messages)
                    {
                        allMessages.Add( message );
                    }
                    iocResolver.Release( handler );
                }
            }
            var tmp = allMessages.ToLookup( m => m.GetCustomAttribute<MessageTopicAttribute>()?.TopicName ?? m.Name )
                .Select( l => new { TopicName = l.Key, MessageType = l.First() } );
            var dic = new Dictionary<string, Type>();
            foreach (var item in tmp)
            {
                if (!dic.ContainsKey( item.TopicName ))
                {
                    dic.Add( item.TopicName, item.MessageType );
                }
            }
            return dic;
        }
    }
}