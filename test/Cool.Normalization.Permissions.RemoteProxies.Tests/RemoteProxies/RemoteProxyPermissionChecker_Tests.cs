using Cool.Normalization.Client;
using cool.permission.client.Api;
using System;
using Shouldly;
using Xunit;

namespace Cool.Normalization.Permissions.RemoteProxies.Tests
{
    public class RemoteProxyPermissionChecker_Tests : NormalizationPermissionRemoteProxyTestBase
    {
        private readonly FakePermissionApi _fakePermissionApi;
        private readonly IProxyPermissionChecker _proxyPermissionChecker;

        public RemoteProxyPermissionChecker_Tests()
        {
            ResolveSelf(ref _fakePermissionApi);
            ResolveSelf(ref _proxyPermissionChecker);
        }

        [Fact]
        public void Test()
        {
            _proxyPermissionChecker.ShouldBeOfType<RemoteProxyPermissionChecker>();
        }
    }

}