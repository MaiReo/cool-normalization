using Abp.Dependency;

namespace Cool.Normalization.Permissions
{
    public class FakePermissionProvider : IPermissionProvider, ISingletonDependency
    {

        public const string Fake = "Fake";
    }
}