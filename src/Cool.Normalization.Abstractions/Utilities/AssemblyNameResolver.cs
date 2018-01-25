using System.Reflection;

namespace Cool.Normalization.Utilities
{
    public class AssemblyNameResolver : IAssemblyNameResolver
    {
        public virtual IAssemblyName ResolveEntryName(Assembly assembly)
        {
            assembly = Assembly.GetEntryAssembly() ?? assembly;
            var uniqueName = assembly.GetName().Name;
            var titleAttr = assembly.GetCustomAttribute<AssemblyProductAttribute>();
            return new AssemblyName( uniqueName, titleAttr?.Product );
        }
    }
}
