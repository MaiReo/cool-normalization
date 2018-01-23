using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public class ConfiguableAssemblyNameResolver : AssemblyNameResolver, IAssemblyNameResolver
    {
        public override IAssemblyName ResolveEntryName(Assembly assembly)
        {
            var name = base.ResolveEntryName( assembly );
            return new AssemblyName( name.UniqueName, new string[]
            {
                NormalizationServiceCollectionExtensions.DisplayName,
                name.DisplayName
            }.FirstOrDefault( s => !string.IsNullOrWhiteSpace( s ) ) );
        }
    }
}
