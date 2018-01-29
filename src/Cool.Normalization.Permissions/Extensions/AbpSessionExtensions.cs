namespace Abp.Runtime.Session
{

    public static class AbpSessionExtensions
    {
        /// <summary>
        /// 获取账号Id
        /// </summary>
        /// <param name="abpSession"></param>
        /// <exception cref="AbpException"></exception>
        /// <returns></returns>
        public static long GetAccountId(this IAbpSession abpSession)
        {
            if (abpSession is IMayHaveAccountIdAbpSession session)
            {
                return session.AccountId 
                    ?? throw new AbpException("There is no AccountId!");
            }
            throw new AbpException( "There is no AccountId!" );
        }
        /// <summary>
        /// 获取旧Main用户Id
        /// </summary>
        /// <param name="abpSession"></param>
        /// <exception cref="AbpException"></exception>
        /// <returns></returns>
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
