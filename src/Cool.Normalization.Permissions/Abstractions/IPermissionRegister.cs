using Cool.Normalization.Permissions;
using System.Collections.Generic;

namespace Cool.Normalization.Permissions
{
    public interface IPermissionRegister
    {
        void Register(string name, string displayName, IEnumerable<CoolPermission> permissions);
    }
}
