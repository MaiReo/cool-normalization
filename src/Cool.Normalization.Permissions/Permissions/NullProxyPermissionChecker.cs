using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization.Permissions
{
    public class NullProxyPermissionChecker : IRemoteProxyPermissionChecker, ISingletonDependency
    {
        public Task<bool> IsGrantedAsync(long accountId, string permissionName, string uniqueName = default( string ))
        {
            return Task.FromResult( true );
        }
    }
}
