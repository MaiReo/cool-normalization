using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Modules
{
    public class MessagePublisherModuleConfiguration : IMessagePublisherModuleConfiguration
    {
        public MessagePublisherModuleConfiguration()
        {
            AutoStart = true;
        }
        public bool AutoStart { get; set; }
    }
}
