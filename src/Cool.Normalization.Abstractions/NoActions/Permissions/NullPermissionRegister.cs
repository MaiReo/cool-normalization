using System.Collections.Generic;

namespace Cool.Normalization.Permissions
{
    public class NullPermissionRegister : IPermissionRegister
    {
        public void Register(string name, string displayName,
            IEnumerable<CoolPermission> permissions)
        {
            //No actions.
        }
    }
}
