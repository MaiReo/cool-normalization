using Abp.Domain.Entities;

namespace Cool.Normalization.Messages.Tests
{
    public class TestMessageEntity : Entity
    {
        public string RequestId { get; set; }

        public string String { get; set; }

        public int Int { get; set; }
    }
}