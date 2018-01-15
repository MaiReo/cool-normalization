using Abp.Dependency;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages.Tests
{
    public class TestMessageReceiverWrapper : IMessageReceiverWrapper, ISingletonDependency
    {
        public bool IsConnected { get; set; }

        public void Connect()
        {
            IsConnected = true;
        }

        public void Disconnect()
        {
            IsConnected = false;
        }
    }
}
