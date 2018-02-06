using Abp.TestBase;
using Abp.Modules;

namespace Cool.Normalization.Permissions.Tests
{
    public abstract class NormalizationPermissionTestBase : AbpIntegratedTestBase<NormalizationPermissionTestModule>
    {
        public NormalizationPermissionTestBase():base()
        {
            
        }

        protected T ResolveSelf<T>(ref T reference) where T : class => reference = Resolve<T>();
    }
}
