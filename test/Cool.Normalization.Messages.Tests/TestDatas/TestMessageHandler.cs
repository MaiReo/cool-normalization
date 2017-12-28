using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages.Tests.TestDatas
{
    public class TestMessageHandler : IMessageHandler<TestMessage>, IMessageHandler
    {
        static TestMessageHandler()
        {
            Messages = new Dictionary<DateTimeOffset, TestMessage>();
        }
        public static Dictionary<DateTimeOffset, TestMessage> Messages { get; private set; }
        public Task HandleMessageAsync( TestMessage message, DateTimeOffset timestamp )
        {
            Messages.Add( timestamp, message );
            return Task.Run( () => message?.ToString() );
        }
    }
}
