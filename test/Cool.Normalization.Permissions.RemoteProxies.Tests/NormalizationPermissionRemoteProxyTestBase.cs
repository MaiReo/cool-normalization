using Abp.TestBase;
using Abp.Modules;

namespace Cool.Normalization.Permissions.RemoteProxies.Tests
{
    public abstract class NormalizationPermissionRemoteProxyTestBase : AbpIntegratedTestBase<NormalizationPermissionRemoteProxyTestModule>
    {
        public NormalizationPermissionRemoteProxyTestBase():base()
        {
            
        }

        protected T ResolveSelf<T>(ref T reference) where T : class => reference = Resolve<T>();
    }
}
