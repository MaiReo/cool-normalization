namespace Abp.Runtime.Session
{
    public interface IMayHaveAccountIdAbpSession : IAbpSession
    {
        long? AccountId { get; }
    }
}
