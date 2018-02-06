using Abp;
using Abp.Modules;
using Cool.Normalization.Permissions;
using Shouldly;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;
using System.Collections.Generic;
using IdentityModel;

namespace Cool.Normalization.Permissions.Tests
{
    public class AccessTokenAbpSession_Tests : NormalizationPermissionTestBase
    {
        private readonly AccessTokenAbpSession _session;
        public AccessTokenAbpSession_Tests()
        {
            ResolveSelf(ref _session);
        }

        public const string ACCOUNT_ID_STRING = "1";

        public const string ABP_USER_ID_STRING = "2";

        protected override void PostInitialize()
        {
            var principalAccessor = Resolve<FakePrincipalAccessor>();
            var identities =  new [] 
            {
                 new ClaimsIdentity(
                     new [] 
                     {
                         new Claim(JwtClaimTypes.Subject,ACCOUNT_ID_STRING),
                         new Claim(NormalizationClaimTypes.AbpUserId,ABP_USER_ID_STRING)
                     },
                     
                     "Bearer")
            };
            principalAccessor.Principal = new ClaimsPrincipal(identities);
        }

        [Fact]
        public void AccountId_Should_Be_1() => _session.AccountId.ShouldBe(int.Parse(ACCOUNT_ID_STRING));

        [Fact]
        public void AbpUserId_Should_Be_2() => _session.AbpUserId.ShouldBe(int.Parse(ABP_USER_ID_STRING));

        [Fact]
        public void UserId_Should_Be_1() => _session.UserId.ShouldBe(int.Parse(ACCOUNT_ID_STRING));
    }
}