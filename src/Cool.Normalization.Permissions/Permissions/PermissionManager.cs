using Abp.Dependency;
using Cool.Normalization.Configuration;
using Cool.Normalization.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cool.Normalization.Permissions
{
    public class PermissionManager : IPermissionManager, ISingletonDependency
    {
        private readonly INormalizationConfiguration _configuration;
        private readonly IAssemblyNameResolver _assemblyNameResolver;
        private readonly IPermissionProvider _permissionProvider;
        private readonly IPermissionRegister _permissionRegister;

        public PermissionManager(
            INormalizationConfiguration configuration,
            IAssemblyNameResolver assemblyNameResolver,
            IPermissionProvider permissionProvider,
            IPermissionRegister permissionRegister
            )
        {
            this._configuration = configuration;

            this._assemblyNameResolver = assemblyNameResolver;
            this._permissionProvider = permissionProvider;
            this._permissionRegister = permissionRegister;
        }

        public void Register()
        {
            if (!_configuration.IsPermissionEnabled)
            {
                return;
            }
            var name = _assemblyNameResolver.ResolveEntryName( Assembly.GetEntryAssembly() );
            var providerType = _permissionProvider.GetType();
            var permissions = RecursiveGetAllPermissions( providerType );
            _permissionRegister.Register( name.UniqueName, name.DisplayName, permissions );
        }

        private static IEnumerable<CoolPermission> RecursiveGetAllPermissions(Type type)
        {
            var permissions = new List<CoolPermission>();
            for (var baseType = type.BaseType; baseType != typeof( object ); baseType = baseType.BaseType)
            {
                permissions.AddRange( GetAllPermissions( baseType ) );
            }
            permissions.AddRange( GetAllPermissions( type ) );
            permissions = permissions.OrderBy( p => p.Name ).ToList();
            return permissions;
        }

        private static IEnumerable<CoolPermission> GetAllPermissions(Type type)
            => type.GetTypeInfo()
                .DeclaredFields
                .Where( f => f.IsPublic )
                .Where( f => f.IsLiteral )
                .Where( f => f.FieldType == typeof( string ) )
                .Select( f => new CoolPermission( f.GetRawConstantValue() as string, f.Name ) )
                .Where( p => !string.IsNullOrWhiteSpace( p.Name ) )
                .ToList();
    }
}
