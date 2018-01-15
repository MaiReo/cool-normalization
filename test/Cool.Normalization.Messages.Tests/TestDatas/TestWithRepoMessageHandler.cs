using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages.Tests
{
    [Code( "20" )]
    public class TestWithRepoMessageHandler : IMessageHandler<TestWithRepoMessage>, IMessageHandler
    {
        private IRepository<TestMessageEntity> _testMessageEntityRepository;

        public TestWithRepoMessageHandler(IRepository<TestMessageEntity> testMessageEntityRepository)
        {
            _testMessageEntityRepository = testMessageEntityRepository;
        }

        static TestWithRepoMessageHandler()
        {
            _messages = new Dictionary<DateTimeOffset, TestWithRepoMessage>();
        }


        private static Dictionary<DateTimeOffset, TestWithRepoMessage> _messages;
        public static IReadOnlyDictionary<DateTimeOffset, TestWithRepoMessage> Messages => _messages;

        [Code( "20" )]
        public virtual Task HandleMessageAsync(TestWithRepoMessage message, DateTimeOffset timestamp)
        {
            _messages.Add( timestamp, message );
            var entity = new TestMessageEntity
            {
                Int = message.Int,
                RequestId = message.RequestId,
                String = message.String
            };
            return _testMessageEntityRepository.InsertAsync( entity );
        }
    }
}
