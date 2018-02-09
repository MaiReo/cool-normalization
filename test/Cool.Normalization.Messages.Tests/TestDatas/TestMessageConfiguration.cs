using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages.Tests
{
    internal sealed class TestMessageConfiguration : MessageConfiguration, IMessageConfiguration
    {
        public override string BrokerAddress => Default.BrokerAddress;
        public override int BrokerPort => Default.BrokerPort;
        public override string ReceiverGroupId => "testhost";
    }
}
