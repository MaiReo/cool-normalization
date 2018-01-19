using Abp.Runtime.Session;
using Cool.Normalization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Runtime.Session
{
    public static class AbpSessionExtensions
    {
        public static long GetAccountId(IAbpSession abpSession)
        {
            if (abpSession is IMayHaveAccountIdAbpSession session)
            {
                return session.AccountId ?? throw new AbpException("There is no AccountId!");
            }
            throw new AbpException( "There is no AccountId!" );
        }

        public static long GetAbpUserId(IAbpSession abpSession)
        {
            if (abpSession is IMayHaveAbpUserIdAbpSession session)
            {
                return session.AbpUserId ?? throw new AbpException( "There is no AbpUserId!" );
            }
            throw new AbpException( "There is no AbpUserId!" );
        }
    }
}
