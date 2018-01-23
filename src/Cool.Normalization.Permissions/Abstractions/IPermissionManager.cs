using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public interface IPermissionManager : ISingletonDependency
    {
        void Register();
    }
}
