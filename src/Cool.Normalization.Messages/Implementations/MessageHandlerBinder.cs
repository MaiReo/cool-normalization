using Abp.Dependency;
using Castle.Core.Logging;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions.Extensions;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages
{
    public class MessageHandlerBinder : IMessageHandlerBinder, ISingletonDependency
    {
        public NullMessageResolver MessageResolver { get; set; }

        public NullMessageHandlerInvoker MessageHandlerInvoker { get; set; }

        public bool Binding(IIocResolver iocResolver, IMessageConfiguration configuration)
        {
            var messages = MessageResolver.HasHandlerMessages( iocResolver );
            if (messages?.Any() != true)
            {
                return false;
            }
            foreach (var topic in messages.Keys)
            {
                configuration.Subscription.Add( topic );
            }
            configuration.MessageReceiving += async (sender, e) =>
                await MessageHandlerInvoker.InvokeAsync( messages, e );
            
            return true;
        }



    }
}