using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using IdentityModel;
using System.Linq;

namespace Cool.Normalization.Permissions
{
    public class AccessTokenAbpSession : ClaimsAbpSession, IAccessTokenAbpSession, IMayHaveAbpUserIdAbpSession, IMayHaveAccountIdAbpSession, IAbpSession, ISingletonDependency
    {
        public AccessTokenAbpSession(IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
            : base( principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider )
        {
        }

        public override long? UserId => this.AccountId ?? this.AbpUserId ?? base.UserId;

        public long? AccountId => this.GetAccountIdFromToken();

        public long? AbpUserId => this.GetAbpUserIdFromToken();

        private long? GetAccountIdFromToken()
        {
            var idString = PrincipalAccessor.Principal?.Claims?.FirstOrDefault(
                c => c.Type == JwtClaimTypes.Subject )?.Value;
            if (long.TryParse( idString, out var accountId ))
            {
                return accountId;
            }
            return default( long? );
        }

        private long? GetAbpUserIdFromToken()
        {
            var idString = PrincipalAccessor.Principal?.Claims?.FirstOrDefault(
                c => c.Type == NormalizationClaimTypes.AbpUserId )?.Value;
            if (long.TryParse( idString, out var accountId ))
            {
                return accountId;
            }
            return default( long? );
        }
    }
}
