using Abp.Modules;
using Abp.Runtime.Session;
using Cool.Normalization.Permissions;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Configuration.Startup;
using System.IdentityModel.Tokens.Jwt;

namespace Cool.Normalization
{
    public class NormalizationPermissionModule : AbpModule
    {
        public override void PreInitialize()
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration.ReplaceService<IAbpSession, AccessTokenAbpSession>();
        }
    }
}
