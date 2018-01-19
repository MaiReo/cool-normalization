using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization
{
    public interface IMayHaveAccountIdAbpSession : IAbpSession
    {
        long? AccountId { get; }
    }
}
