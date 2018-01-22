using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization
{
    public interface IPermissionRegister
    {
        void Register(string name, string displayName, IEnumerable<Permission> permissions);
    }
}
