using System.Reflection;

namespace Cool.Normalization.Utilities
{
    public interface IAssemblyNameResolver
    {
        IAssemblyName ResolveEntryName(Assembly assembly);
    }
}
