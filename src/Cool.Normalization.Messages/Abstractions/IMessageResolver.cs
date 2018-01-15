using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Messages
{
    public interface IMessageResolver
    {
        IReadOnlyDictionary<string, Type> HasHandlerMessages( IIocResolver iocResolver );
    }
}
