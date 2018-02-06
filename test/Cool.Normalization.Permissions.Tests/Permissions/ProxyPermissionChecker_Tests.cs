using Abp;
using Abp.Authorization;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;


namespace Cool.Normalization.Permissions.Tests
{
    public class ProxyPermissionChecker_Tests : NormalizationPermissionTestBase
    {
        private readonly FakeProxyPermissionChecker _fakeProxyPermissionChecker;

        private readonly IPermissionChecker _proxyPermissionChecker;
        public ProxyPermissionChecker_Tests()
        {
            ResolveSelf(ref _fakeProxyPermissionChecker);
            ResolveSelf(ref _proxyPermissionChecker);
        }
        public const long ACCOUNT_ID = 1L;

        [Fact]
        public async Task IsGrantAsync_Test()
        {
            _proxyPermissionChecker.ShouldBeOfType<ProxyPermissionChecker>();

            _fakeProxyPermissionChecker.GrantPermission(ACCOUNT_ID, "Fake");

            var isGrant = await _proxyPermissionChecker.IsGrantedAsync(new UserIdentifier(1, ACCOUNT_ID), "Fake");

            isGrant.ShouldBe(true);
        }
    }
}