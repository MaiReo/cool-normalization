namespace Abp.Runtime.Session
{

    public static class AbpSessionExtensions
    {
        public static long GetAccountId(this IAbpSession abpSession)
        {
            if (abpSession is IMayHaveAccountIdAbpSession session)
            {
                return session.AccountId 
                    ?? throw new AbpException("There is no AccountId!");
            }
            throw new AbpException( "There is no AccountId!" );
        }

        public static long GetAbpUserId(this IAbpSession abpSession)
        {
            if (abpSession is IMayHaveAbpUserIdAbpSession session)
            {
                return session.AbpUserId 
                    ?? throw new AbpException( "There is no AbpUserId!" );
            }
            throw new AbpException( "There is no AbpUserId!" );
        }
    }
}
