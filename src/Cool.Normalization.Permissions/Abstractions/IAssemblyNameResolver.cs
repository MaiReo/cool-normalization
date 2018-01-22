using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public interface IAssemblyNameResolver
    {
        IAssemblyName ResolveEntryName(Assembly assembly);
    }
}
