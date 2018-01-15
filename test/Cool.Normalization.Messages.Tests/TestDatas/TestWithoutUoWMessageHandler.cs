using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages.Tests
{
    [Code( "30" )]
    public class TestWithoutUoWMessageHandler : IMessageHandler<TestWithoutUowMessage>, IMessageHandler
    {
        private IRepository<TestMessageEntity> _testMessageEntityRepository;

        public TestWithoutUoWMessageHandler(IRepository<TestMessageEntity> testMessageEntityRepository)
        {
            _testMessageEntityRepository = testMessageEntityRepository;
        }

        static TestWithoutUoWMessageHandler()
        {
            _messages = new Dictionary<DateTimeOffset, TestWithoutUowMessage>();
        }


        private static Dictionary<DateTimeOffset, TestWithoutUowMessage> _messages;
        public static IReadOnlyDictionary<DateTimeOffset, TestWithoutUowMessage> Messages => _messages;

        [Code( "40" )]
        public async virtual Task HandleMessageAsync(TestWithoutUowMessage message, DateTimeOffset timestamp)
        {
            _messages.Add( timestamp, message );
            var entity = new TestMessageEntity
            {
                Int = message.Int,
                RequestId = message.RequestId,
                String = message.String
            };
            await _testMessageEntityRepository.InsertAsync( entity );
            throw new NormalizationException("60","10");
        }
    }
}
