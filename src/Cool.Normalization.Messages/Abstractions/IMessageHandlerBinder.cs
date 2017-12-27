using Abp.Dependency;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages
{
    public interface IMessageHandlerBinder
    {
        bool Binding( IIocResolver iocResolver, IMessageConfiguration configuration );
    }
}
