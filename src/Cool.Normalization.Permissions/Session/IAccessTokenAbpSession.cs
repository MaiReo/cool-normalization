namespace Abp.Runtime.Session
{
    public interface IAccessTokenAbpSession : IMayHaveAbpUserIdAbpSession, IMayHaveAccountIdAbpSession, IAbpSession
    {
    }
}
