using MaiReo.Messages.Abstractions;
using MaiReo.Messages.Abstractions.Events;
using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions.Extensions;
using System.Reflection;
using System.Reflection.Extensions;
using System.Threading.Tasks;
using Xunit;
using System.Linq.Extensions;

namespace Cool.Normalization.Messages.Tests
{
    public class Message_Tests : MessageTestBase
    {
        [Fact]
        public void Resolver()
        {
            var resolver = default( NullMessageResolver );
            try
            {
                resolver = LocalIocManager.Resolve<NullMessageResolver>();
                var messages = resolver.HasHandlerMessages( LocalIocManager );
                messages["TestCoolMessage"].ShouldBe( typeof( TestMessage ) );
                messages.Count.ShouldBeGreaterThanOrEqualTo( 1 );
            }
            finally
            {
                if (resolver != null)
                    LocalIocManager.Release( resolver );
            }
        }

        [Fact]
        public void HandlerResolver()
        {
            var resolver = default( NullMessageHandlerResolver );
            var handler = default( object );
            try
            {
                resolver = LocalIocManager.Resolve<NullMessageHandlerResolver>();
                handler = resolver.Resolve( LocalIocManager, typeof( TestMessage ) );
                handler.ShouldBeAssignableTo<IMessageHandler<TestMessage>>();

            }
            finally
            {
                if (resolver != null)
                    LocalIocManager.Release( resolver );
                if (handler != null)
                    LocalIocManager.Release( handler );
            }
        }

        [Fact]
        public async Task ExpressionBuilder()
        {
            var resolver = LocalIocManager.Resolve<NullMessageHandlerResolver>();
            var builder = LocalIocManager.Resolve<NullMessageHandlerCallExpressionBuilder>();
            var handler = default( object );
            var message = new TestMessage
            {
                Int = 1,
                String = "string",
                RequestId = Guid.NewGuid().ToString( "N" ).ToLowerInvariant(),
                DateTimeOffset = DateTimeOffset.UtcNow,
            };
            var messageJson = Newtonsoft.Json.JsonConvert.SerializeObject( message );
            var wrapper = new MessageWrapper( "TestCoolMessage", messageJson, message.DateTimeOffset );
            try
            {
                handler = resolver.Resolve( LocalIocManager, typeof( TestMessage ) );
                var expr = builder.Build( typeof( TestMessage ), wrapper );
                var ifaceMethod = typeof( IMessageHandler<TestMessage> ).GetMethod( nameof( IMessageHandler<TestMessage>.HandleMessageAsync ) );
                var implMethod = typeof( TestMessageHandler ).GetMethod( nameof( IMessageHandler<TestMessage>.HandleMessageAsync ) );
                var exprMethod = expr.AsMethod();
                exprMethod.ShouldBe( ifaceMethod );
                exprMethod.ImplementationAt( handler ).ShouldBe( implMethod );

                var func = expr.Compile();
                var task = func.Invoke( handler );
                await task;
                TestMessageHandler.Messages.ShouldNotBeEmpty();
                var firstMessage = TestMessageHandler.Messages[message.DateTimeOffset];
                firstMessage.RequestId.ShouldBe( message.RequestId );
                firstMessage.Int.ShouldBe( message.Int );
                firstMessage.String.ShouldBe( message.String );
                firstMessage.DateTimeOffset.ShouldBe( message.DateTimeOffset );
                if (task is Task<string> taskReturnString)
                {
                    taskReturnString.Result.ShouldBe( message.ToString() );
                }
            }
            finally
            {
                if (builder != null)
                    LocalIocManager.Release( builder );
                if (resolver != null)
                    LocalIocManager.Release( resolver );
                if (handler != null)
                    LocalIocManager.Release( handler );
            }
        }
        [Fact]
        public void LoggerFormat()
        {
            var message = new TestMessage
            {
                Int = 1,
                String = "string",
                RequestId = Guid.NewGuid().ToString( "N" ).ToLowerInvariant(),
                DateTimeOffset = DateTimeOffset.UtcNow,
            };
            var messageJson = Newtonsoft.Json.JsonConvert.SerializeObject( message );
            var formatter = default( IMessageLogFormatter );
            try
            {
                formatter = LocalIocManager.Resolve<IMessageLogFormatter>();
                var handler = LocalIocManager.Resolve<IMessageHandler<TestMessage>>();
                var handlerType = handler.GetType();
                LocalIocManager.Release( handler );
                var wrapper = new MessageWrapper( "TestCoolMessage", messageJson, message.DateTimeOffset );
                var ifaceMethod = typeof( IMessageHandler<TestMessage> ).GetMethod( nameof( IMessageHandler<TestMessage>.HandleMessageAsync ) );
                var implMethod = handlerType.GetMethod( nameof( IMessageHandler<TestMessage>.HandleMessageAsync ) );
                var log = formatter.Format( wrapper, 1000,
                    ifaceMethod, implMethod
                    );
                log.ShouldBe( $"MESSAGE-HANDLED^1000^00101000^{handlerType.FullName}^TestCoolMessage^{message.DateTimeOffset}^{messageJson}^NULL" );
            }
            finally
            {
                if (formatter != null)
                    LocalIocManager.Release( formatter );
            }
        }

        [Fact]
        public async Task Binder()
        {
            IMessageConfiguration config = new TestMessageConfiguration();
            var binder = default( IMessageHandlerBinder );
            try
            {
                binder = LocalIocManager.Resolve<IMessageHandlerBinder>();
                var bindResult = binder.Binding( LocalIocManager, config );

                bindResult.ShouldBe( config.Subscription.Any() );

                var oldCount = TestMessageHandler.Messages.Count;
                await config.OnMessageReceivingAsync( new MessageReceivingEventArgs( new MessageWrapper( "TestCoolMessage", "{}" ) ) );
                var newCount = TestMessageHandler.Messages.Count;
                if (bindResult)
                {
                    newCount.ShouldBeGreaterThan( oldCount );
                }
                else
                {
                    newCount.ShouldBe( oldCount );
                }

            }
            finally
            {
                if (binder != default( IMessageHandlerBinder ))
                    LocalIocManager.Release( binder );
            }
        }

        [Fact]
        public async Task Invoker()
        {
            var topic = typeof( TestMessage ).GetCustomAttribute<MessageTopicAttribute>().TopicName;
            var messages = new Dictionary<string, Type>()
            {
                { topic, typeof(TestMessage) }
            };
            var wrapper = new MessageWrapper( topic, "{}", DateTimeOffset.UtcNow );
            var invoker = default( NullMessageHandlerInvoker );
            var oldCount = TestMessageHandler.Messages.Count;
            try
            {
                invoker = LocalIocManager.Resolve<NullMessageHandlerInvoker>();
                await invoker.InvokeAsync( messages, wrapper );
                var newCount = TestMessageHandler.Messages.Count;

                newCount.ShouldBeGreaterThan( oldCount );
            }
            finally
            {
                if (invoker != default( NullMessageHandlerInvoker ))
                    LocalIocManager.Release( invoker );
            }
        }
        [Fact]
        public void ResolveSuccessCode()
        {
            var codeResolver = default( IMessageHandlerCodeResolver );
            var ifaceMethod = typeof( IMessageHandler<TestMessage> ).GetMethod( nameof( IMessageHandler<TestMessage>.HandleMessageAsync ) );
            var implMethod = typeof( TestMessageHandler ).GetMethod( nameof( IMessageHandler<TestMessage>.HandleMessageAsync ) );
            try
            {
                codeResolver = LocalIocManager.Resolve<IMessageHandlerCodeResolver>();
                var successCode = codeResolver.ResolveCode( ifaceMethod, implMethod );
                successCode.CombineCodes().ShouldBe( "00101000" );
            }
            finally
            {
                if (codeResolver != null)
                    LocalIocManager.Release( codeResolver );
            }
        }

        [Fact]
        public void ResolveSuccessCodeUnderlyingCastleProxy()
        {
            var codeResolver = default( IMessageHandlerCodeResolver );
            var ifaceMethod = typeof( IMessageHandler<TestWithRepoMessage> ).GetMethod( nameof( IMessageHandler<TestWithRepoMessage>.HandleMessageAsync ) );
            var implInst = LocalIocManager.Resolve<IMessageHandler<TestWithRepoMessage>>();
            var proxyType = implInst.GetType();
            LocalIocManager.Release( implInst );
            var implMethod = proxyType.GetMethod( nameof( IMessageHandler<TestWithRepoMessage>.HandleMessageAsync ) );

            try
            {
                codeResolver = LocalIocManager.Resolve<IMessageHandlerCodeResolver>();
                var successCode = codeResolver.ResolveCode( ifaceMethod, implMethod );
                successCode.CombineCodes().ShouldBe( "00202000" );
            }
            finally
            {
                if (codeResolver != null)
                    LocalIocManager.Release( codeResolver );
            }
        }

        [Fact]
        public void ResolveErrorCode()
        {
            var codeResolver = default( IMessageHandlerCodeResolver );
            var ifaceMethod = typeof( IMessageHandler<TestMessage> ).GetMethod( nameof( IMessageHandler<TestMessage>.HandleMessageAsync ) );
            var implMethod = typeof( TestMessageHandler ).GetMethod( nameof( IMessageHandler<TestMessage>.HandleMessageAsync ) );
            try
            {
                codeResolver = LocalIocManager.Resolve<IMessageHandlerCodeResolver>();
                var exception = new NormalizationException( "40", "10" );
                var successCode = codeResolver.ResolveCode( ifaceMethod, implMethod, exception );
                successCode.CombineCodes().ShouldBe( "10101040" );
            }
            finally
            {
                if (codeResolver != null)
                    LocalIocManager.Release( codeResolver );
            }
        }

        [Fact]
        public async Task WithRepository()
        {
            IMessageConfiguration config = new TestMessageConfiguration();
            var binder = default( IMessageHandlerBinder );
            try
            {
                binder = LocalIocManager.Resolve<IMessageHandlerBinder>();
                var bindResult = binder.Binding( LocalIocManager, config );

                var message = new TestWithRepoMessage
                {
                    Int = 21,
                    RequestId = Guid.NewGuid().ToString( "N" ),
                    String = "GUID"
                };
                await config.OnMessageReceivingAsync(
                    new MessageReceivingEventArgs(
                        new MessageWrapper( nameof( TestWithRepoMessage )
                        , message.ToJson() ) ) );

                using (var context = LocalIocManager.Resolve<TestDbContext>())
                {
                    context.TestMessageEntities.Any( e =>
                    e.Int == message.Int
                    && e.RequestId == message.RequestId
                    && e.String == message.String )
                    .ShouldBeTrue();
                }

            }
            finally
            {
                if (binder != default( IMessageHandlerBinder ))
                    LocalIocManager.Release( binder );
            }
        }

        [Fact]
        public async Task WithUoW()
        {
            IMessageConfiguration config = new TestMessageConfiguration();
            var binder = default( IMessageHandlerBinder );
            try
            {
                binder = LocalIocManager.Resolve<IMessageHandlerBinder>();
                var bindResult = binder.Binding( LocalIocManager, config );

                var message = new TestWithUowMessage
                {
                    Int = 22,
                    RequestId = Guid.NewGuid().ToString( "N" ),
                    String = "GUID_UoW"
                };
                await config.OnMessageReceivingAsync(
                    new MessageReceivingEventArgs(
                        new MessageWrapper( nameof( TestWithUowMessage )
                        , message.ToJson() ) ) );

                using (var context = LocalIocManager.Resolve<TestDbContext>())
                {
                    context.TestMessageEntities.Any( e =>
                    e.Int == message.Int
                    && e.RequestId == message.RequestId
                    && e.String == message.String )
                    .ShouldBeFalse();
                }

            }
            finally
            {
                if (binder != default( IMessageHandlerBinder ))
                    LocalIocManager.Release( binder );
            }
        }

        [Fact]
        public async Task WithoutUoW()
        {
            IMessageConfiguration config = new TestMessageConfiguration();
            var binder = default( IMessageHandlerBinder );
            try
            {
                binder = LocalIocManager.Resolve<IMessageHandlerBinder>();
                var bindResult = binder.Binding( LocalIocManager, config );

                var message = new TestWithoutUowMessage
                {
                    Int = 23,
                    RequestId = Guid.NewGuid().ToString( "N" ),
                    String = "GUID_No_UoW"
                };
                await config.OnMessageReceivingAsync(
                    new MessageReceivingEventArgs(
                        new MessageWrapper( nameof( TestWithoutUowMessage )
                        , message.ToJson() ) ) );

                using (var context = LocalIocManager.Resolve<TestDbContext>())
                {
                    context.TestMessageEntities.Any( e =>
                    e.Int == message.Int
                    && e.RequestId == message.RequestId
                    && e.String == message.String )
                    .ShouldBeTrue();
                }

            }
            finally
            {
                if (binder != default( IMessageHandlerBinder ))
                    LocalIocManager.Release( binder );
            }
        }
    }
}
