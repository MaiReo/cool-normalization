using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cool.Normalization.Messages.Tests
{
    [Code( "30" )]
    public class TestWithUoWMessageHandler : IMessageHandler<TestWithUowMessage>, IMessageHandler
    {
        private IRepository<TestMessageEntity> _testMessageEntityRepository;

        public TestWithUoWMessageHandler(IRepository<TestMessageEntity> testMessageEntityRepository)
        {
            _testMessageEntityRepository = testMessageEntityRepository;
        }

        static TestWithUoWMessageHandler()
        {
            _messages = new Dictionary<DateTimeOffset, TestWithUowMessage>();
        }


        private static Dictionary<DateTimeOffset, TestWithUowMessage> _messages;
        public static IReadOnlyDictionary<DateTimeOffset, TestWithUowMessage> Messages => _messages;

        [Code( "30" )]
        [UnitOfWork]
        public async virtual Task HandleMessageAsync(TestWithUowMessage message, DateTimeOffset timestamp)
        {
            _messages.Add( timestamp, message );
            var entity = new TestMessageEntity
            {
                Int = message.Int,
                RequestId = message.RequestId,
                String = message.String
            };
            await _testMessageEntityRepository.InsertAsync( entity );
            throw new NormalizationException("50","10");
        }
    }
}
