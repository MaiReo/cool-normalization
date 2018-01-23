using Abp.Modules;
using Abp.Runtime.Session;
using Cool.Normalization.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Configuration.Startup;
using System.IdentityModel.Tokens.Jwt;
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
            Configuration.ReplaceService<Abp.Authorization.IPermissionChecker, ProxyPermissionChecker>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention( typeof( NormalizationPermissionModule ).Assembly );
        }

        public override void PostInitialize()
        {
            IocManager.RegisterIfNot<IPermissionProvider, NullPermissionProvider>();

            using (var permissionManager = IocManager.ResolveAsDisposable<IPermissionManager>())
            {
                permissionManager.Object.Register();
            }
        }
    }
}
