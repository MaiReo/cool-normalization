using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions.Extensions;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Castle.Core.Logging;
using MaiReo.Messages.Abstractions;
using System.Reflection.Extensions;
using System.Reflection;
using Abp.Domain.Uow;
using Abp.Configuration.Startup;

namespace Cool.Normalization.Messages
{
    public class MessageHandlerInvoker : IMessageHandlerInvoker
    {
        public IMessageHandlerResolver MessageHandlerResolver { get; set; }
        public IMessageHandlerCallExpressionBuilder MessageHandlerBuilder { get; set; }
        public IMessageLogFormatter MessageLogFormatter { get; set; }
        public IIocResolver IocResolver { get; set; }
        public ILogger Logger { get; set; }

        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public async Task InvokeAsync(
            IReadOnlyDictionary<string, Type> messages,
            IMessageWrapper wrapper)
        {
            var stopWatch = Stopwatch.StartNew();
            var handlerInstance = default( object );
            var exception = default( Exception );
            var messageType = default( Type );
            var ifaceMethod = default( MethodInfo );
            var handlerMethod = default( MethodInfo );
            try
            {
                if (messages?.TryGetValue( wrapper?.Topic ?? string.Empty,
                        out messageType ) != true)
                {
                    return;
                }
                handlerInstance = MessageHandlerResolver?.Resolve( IocResolver, messageType );

                var handlerCallExpression
                        = MessageHandlerBuilder?.Build( messageType, wrapper );

                ifaceMethod = handlerCallExpression?.AsMethod();
                handlerMethod = ifaceMethod?.ImplementationAt( handlerInstance );

                var @delegate = handlerCallExpression?.Compile();

                await (@delegate?.Invoke( handlerInstance ) ?? Task.CompletedTask);

            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                if (handlerInstance != null)
                {
                    IocResolver.Release( handlerInstance );
                }

                stopWatch.Stop();

                var log = MessageLogFormatter?.Format( wrapper,
                    stopWatch.ElapsedMilliseconds,
                     ifaceMethod, handlerMethod,
                     exception );

                if (!string.IsNullOrWhiteSpace( log ))
                {
                    Logger?.Info( log );
                }


            }

        }
    }
}
