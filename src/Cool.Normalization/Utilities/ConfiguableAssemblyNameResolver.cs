﻿using Abp.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
namespace Cool.Normalization.Utilities
{
    public class ConfiguableAssemblyNameResolver
        : AssemblyNameResolver, IAssemblyNameResolver, ISingletonDependency
    {
        public override IAssemblyName ResolveEntryName(Assembly assembly)
        {
            var name = base.ResolveEntryName( assembly );
            return new AssemblyName( name.UniqueName, new string[]
            {
                NormalizationServiceCollectionExtensions.FriendlyName,
                name.DisplayName
            }.FirstOrDefault( s => !string.IsNullOrWhiteSpace( s ) ) );
        }
    }
}
