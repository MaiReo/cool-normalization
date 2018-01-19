using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using IdentityModel;
using System.Linq;

namespace Cool.Normalization.Permissions
{
    public class AccessTokenAbpSession : ClaimsAbpSession, IAbpSession, ISingletonDependency
    {
        public AccessTokenAbpSession(IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
            : base( principalAccessor, multiTenancy, tenantResolver, sessionOverrideScopeProvider )
        {
        }

        public override long? UserId => this.GetUserIdFromToken() ?? base.UserId;

        private long? GetUserIdFromToken()
        {
            var accountIdString = PrincipalAccessor.Principal?.Claims?.FirstOrDefault( c => c.Type == JwtClaimTypes.Subject )?.Value;
            if (long.TryParse( accountIdString, out var accountId ))
            {
                return accountId;
            }
            return default( long? );
        }
    }
}
