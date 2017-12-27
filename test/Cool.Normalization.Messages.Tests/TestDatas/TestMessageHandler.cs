using MaiReo.Messages.Abstractions.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages.Tests.TestDatas
{
    public class TestMessageHandler : IMessageHandler<TestMessage>, IMessageHandler
    {
        static TestMessageHandler()
        {
            Messages = new List<TestMessage>();
        }
        public static List<TestMessage> Messages { get; private set; }
        public Task HandleMessageAsync( TestMessage message )
        {
            Messages.Add( message );
            return Task.Run( () => message?.ToString() );
        }
    }
}
