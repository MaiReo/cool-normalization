using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Modules
{
    public interface IMessagePublisherModuleConfiguration
    {
        bool AutoStart { get; set; }
    }
}
