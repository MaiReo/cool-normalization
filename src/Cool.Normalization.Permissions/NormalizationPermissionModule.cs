using Abp.Modules;
using Abp.Runtime.Session;
using Cool.Normalization.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Configuration.Startup;
using System.IdentityModel.Tokens.Jwt;
using Abp.Authorization;
using Abp.Dependency;
using Abp.MultiTenancy;

namespace Cool.Normalization
{
    public class NormalizationPermissionModule : AbpModule
    {

        public override void PreInitialize()
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration.ReplaceService<IAbpSession, AccessTokenAbpSession>();
            Configuration.ReplaceService<IPermissionChecker, ProxyPermissionChecker>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationPermissionModule ).Assembly );
        }

        public override void PostInitialize()
        {
            using (var nameResolver = IocManager.ResolveAsDisposable<IAssemblyNameResolver>())
            using (var permissionManager = IocManager.ResolveAsDisposable<IPermissionManager>())
            using (var permissionRegister = IocManager.ResolveAsDisposable<IPermissionRegister>())
            {
                var assemblyName = nameResolver.Object.ResolveEntryName(
                    typeof( NormalizationPermissionModule ).Assembly );
                permissionRegister.Object.Register(
                    assemblyName.UniqueName,
                    assemblyName.DisplayName,
                    permissionManager.Object.GetAllPermissions( false ) );
            }
        }
    }
}
