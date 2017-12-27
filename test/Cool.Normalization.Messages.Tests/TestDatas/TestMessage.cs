using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages.Tests.TestDatas
{
    [MessageTopic( "TestCoolMessage" )]
    public class TestMessage : IMessage
    {
        public string RequestId { get; set; }

        public string String { get; set; }

        public int Int { get; set; }

        public DateTimeOffset DateTimeOffset { get; set; }

        public override string ToString()
        {
            return $"{nameof( RequestId )}:{RequestId}|{nameof( Int )}:{Int}|{nameof( String )}:{String}|{nameof( DateTimeOffset )}:{DateTimeOffset}";
        }
    }
}
