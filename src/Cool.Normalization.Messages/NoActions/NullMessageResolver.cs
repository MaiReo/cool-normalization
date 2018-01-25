using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages
{
    public class NullMessageResolver : IMessageResolver
    {
        public IReadOnlyDictionary<string, Type> HasHandlerMessages(
            IIocResolver iocResolver)
            => new Dictionary<string, Type>();
    }
}
