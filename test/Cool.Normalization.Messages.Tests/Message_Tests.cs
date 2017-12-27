using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using Cool.Normalization.Messages.Tests.TestDatas;
using MaiReo.Messages.Abstractions.Core;
using System.Linq;
using MaiReo.Messages.Abstractions;

namespace Cool.Normalization.Messages.Tests
{
    public class Message_Tests : MessageTestBase
    {
        [Fact]
        public void Resolver()
        {
            var resolver = default( IMessageResolver );
            try
            {
                resolver = LocalIocManager.Resolve<IMessageResolver>();
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
            var resolver = default( IMessageHandlerResolver );
            var handler = default( object );
            try
            {
                resolver = LocalIocManager.Resolve<IMessageHandlerResolver>();
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
            var resolver = LocalIocManager.Resolve<IMessageHandlerResolver>();
            var builder = LocalIocManager.Resolve<IMessageHandlerCallExpressionBuilder>();
            var handler = default( object );
            var message = new TestMessage
            {
                Int = 1,
                String = "string",
                RequestId = Guid.NewGuid().ToString( "N" ).ToLowerInvariant(),
                DateTimeOffset = DateTimeOffset.UtcNow,
            };
            var messageJson = Newtonsoft.Json.JsonConvert.SerializeObject( message );
            try
            {
                handler = resolver.Resolve( LocalIocManager, typeof( TestMessage ) );
                var expr = builder.Build( typeof( TestMessage ), messageJson );
                var func = expr.Compile();
                var task = func.Invoke( handler );
                await task;
                TestMessageHandler.Messages.ShouldNotBeEmpty();
                var firstMessage = TestMessageHandler.Messages.First( x => x.RequestId == message.RequestId );
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
                var log = formatter.Format( wrapper, 1000, typeof( TestMessage ), handlerType );
                log.ShouldBe( $"MESSAGE-HANDLED|Cool.Normalization.Messages.Tests.TestDatas.TestMessage|{handlerType.FullName}|1000|TestCoolMessage|{message.DateTimeOffset}|{messageJson}|NULL" );
            }
            finally
            {
                if (formatter != null)
                    LocalIocManager.Release( formatter );
            }
        }
    }
}
