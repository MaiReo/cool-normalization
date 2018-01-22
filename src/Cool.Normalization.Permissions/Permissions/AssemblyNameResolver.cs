using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public class AssemblyNameResolver : IAssemblyNameResolver, ISingletonDependency
    {
        public IAssemblyName ResolveEntryName(Assembly assembly)
        {
            assembly = Assembly.GetEntryAssembly() ?? assembly;
            var uniqueName = assembly.GetName().Name;
            var titleAttr = assembly.GetCustomAttribute<AssemblyProductAttribute>();
            return new AssemblyName( uniqueName, titleAttr?.Product );
        }
    }
}
