using Abp.Dependency;
using MaiReo.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages
{
    public class NullMessageHandlerBinder : IMessageHandlerBinder
    {
        public bool Binding(IIocResolver iocResolver,
            IMessageConfiguration configuration)
            => false;

        public static NullMessageHandlerBinder Instance 
            => new NullMessageHandlerBinder();
    }
}
