using Abp.Dependency;
using Castle.Core.Logging;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages
{
    public class MessageHandlerBinder : IMessageHandlerBinder
    {
        public IMessageResolver MessageResolver { get; set; }
        public IMessageHandlerResolver MessageHandlerResolver { get; set; }
        public IMessageHandlerCallExpressionBuilder MessageHandlerCallExpressionBuilder { get; set; }
        public IMessageLogFormatter MessageLogFormatter { get; set; }

        public ILogger Logger { get; set; }

        public bool Binding( IIocResolver iocResolver, IMessageConfiguration configuration )
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

            configuration.MessageReceiving += ( sender, e ) =>
            {
                Task.Run( async () =>
                {
                    var stopWatch = Stopwatch.StartNew();
                    var handlerInstance = default( object );
                    var exception = default( Exception );
                    var messageType = default( Type );
                    try
                    {
                        if (!messages.TryGetValue( e.Topic, out messageType ))
                        {
                            return;
                        }
                        handlerInstance = MessageHandlerResolver.Resolve( iocResolver, messageType );

                        var handlerCallExpression = MessageHandlerCallExpressionBuilder.Build( messageType, e.Message, e.Timestamp );

                        var @delegate = handlerCallExpression.Compile();

                        await @delegate.Invoke( handlerInstance );

                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }
                    finally
                    {
                        var handlerInstanceType =
                        handlerInstance?.GetType();
                        if (handlerInstance != null)
                        {
                            iocResolver.Release( handlerInstance );
                        }
                        stopWatch.Stop();

                        var log = MessageLogFormatter.Format( e, stopWatch.ElapsedMilliseconds, messageType, handlerInstanceType, exception );
                        Logger.Info( log );

                    }

                } );
            };
            return true;
        }



    }
}