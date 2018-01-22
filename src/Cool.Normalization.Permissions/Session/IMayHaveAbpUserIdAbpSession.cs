using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Runtime.Session
{
    public interface IMayHaveAbpUserIdAbpSession : IAbpSession
    {
        long? AbpUserId { get; }
    }
}
