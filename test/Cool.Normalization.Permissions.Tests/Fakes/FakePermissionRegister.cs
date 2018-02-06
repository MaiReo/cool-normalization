using System.Collections.Concurrent;
using System.Collections.Generic;
using Abp.Dependency;

namespace Cool.Normalization.Permissions
{
    public class FakePermissionRegister : IPermissionRegister, ISingletonDependency
    {
        public class RegisterInput
        {
            public RegisterInput(string name, string displayName, IEnumerable<CoolPermission> permissions)
            {
                this.Name = name;
                this.DisplayName = displayName;
                this.Permissions = permissions;
            }
            public string Name { get; set; }

            public string DisplayName { get; set; }

            public IEnumerable<CoolPermission> Permissions { get; set; }
        }
        public ConcurrentBag<RegisterInput> RegisterInputs { get; }

        public FakePermissionRegister()
        {
            RegisterInputs = new ConcurrentBag<RegisterInput>();
        }
        public void Register(string name, string displayName, IEnumerable<CoolPermission> permissions)
        {
            RegisterInputs.Add(new RegisterInput(name, displayName, permissions));
        }
    }
}