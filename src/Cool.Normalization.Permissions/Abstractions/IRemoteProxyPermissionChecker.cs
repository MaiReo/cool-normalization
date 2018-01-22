using Abp.Authorization;
using Abp.Dependency;
using Cool.Normalization.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Normalization.Permissions
{
    public interface IRemoteProxyPermissionChecker
    {
        Task<bool> IsGrantedAsync(long accountId, string permissionName, string uniqueName = default( string ));
    }
}
