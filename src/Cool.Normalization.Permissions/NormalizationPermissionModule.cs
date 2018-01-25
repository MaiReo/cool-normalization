using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Runtime.Session;
using Cool.Normalization.Permissions;
using System.IdentityModel.Tokens.Jwt;

namespace Abp.Modules
{
    [DependsOn( typeof( NormalizationAbstractionModule ) )]
    public class NormalizationPermissionModule : AbpModule
    {

        public override void PreInitialize()
        {
            Configuration.ReplaceService<IAbpSession, AccessTokenAbpSession>();
            Configuration.ReplaceService<Abp.Authorization.IPermissionChecker,
                ProxyPermissionChecker>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(
                typeof( NormalizationPermissionModule ).Assembly );
            if (Configuration.Modules.Normalization().IsPermissionEnabled)
            {
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            }
        }

        public override void PostInitialize()
        {
            using (var permissionManager = IocManager
                    .ResolveAsDisposable<IPermissionManager>())
            {
                permissionManager.Object.Register();
            }

        }
    }
}
