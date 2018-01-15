using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages.Tests
{
    [Code("10")]
    public class TestMessageHandler : IMessageHandler<TestMessage>, IMessageHandler
    {
        static TestMessageHandler()
        {
            Messages = new Dictionary<DateTimeOffset, TestMessage>();
        }
        public static Dictionary<DateTimeOffset, TestMessage> Messages { get; private set; }

        [Code( "10" )]
        public Task HandleMessageAsync( TestMessage message, DateTimeOffset timestamp )
        {
            Messages.Add( timestamp, message );
            return Task.Run( () => message?.ToString() );
        }
    }
}
