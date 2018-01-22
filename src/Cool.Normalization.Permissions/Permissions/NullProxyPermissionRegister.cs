using Abp.Authorization;
using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public class NullProxyPermissionRegister : IPermissionRegister, ISingletonDependency
    {
        public void Register(string name, string displayName, IEnumerable<Permission> permissions)
        {
            //No actions.
        }
    }
}
