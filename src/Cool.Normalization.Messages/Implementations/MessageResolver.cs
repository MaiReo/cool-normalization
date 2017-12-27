using System;
using System.Collections.Generic;
using Abp.Dependency;
using MaiReo.Messages.Abstractions.Core;
using System.Linq;
using System.Reflection;
using MaiReo.Messages.Abstractions;

namespace Cool.Normalization.Messages
{
    public class MessageResolver : IMessageResolver
    {
        public IDictionary<string, Type> HasHandlerMessages(
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
            var tmp = allMessages.ToLookup( m => m.GetCustomAttribute<MessageTopicAttribute>() )
                .Where( l => l.Key != null && (!string.IsNullOrWhiteSpace( l.Key.TopicName )) )
                .Select( l => new { l.Key.TopicName, MessageType = l.First() } );
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