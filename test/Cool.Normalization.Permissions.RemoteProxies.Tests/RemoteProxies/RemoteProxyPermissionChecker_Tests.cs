using Cool.Normalization.Client;
using cool.permission.client.Api;
using System;
using Shouldly;
using Xunit;
using System.Threading.Tasks;

namespace Cool.Normalization.Permissions.RemoteProxies.Tests
{
    public class RemoteProxyPermissionChecker_Tests
        : NormalizationPermissionRemoteProxyTestBase
    {
        private readonly FakePermissionApi _fakePermissionApi;
        private readonly IProxyPermissionChecker _proxyPermissionChecker;

        public RemoteProxyPermissionChecker_Tests()
        {
            ResolveSelf( ref _fakePermissionApi );
            ResolveSelf( ref _proxyPermissionChecker );
        }

        [Fact( DisplayName = "远程代理_已授权" )]
        public async Task IsGrantedAsync_Test()
        {
            //Assert
            _proxyPermissionChecker.ShouldBeOfType
              <RemoteProxyPermissionChecker>();
            //Arrange
            var original_delegate = _fakePermissionApi
                .IsGrantWithHttpInfo_Delegate;
            _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                = input => new NormalizationResponse
                <cool.permission.client.Model.IsGrantOutput>
                {
                    Code = "00010100",
                    __normalization = true,
                    Data = new cool.permission.client.Model.IsGrantOutput
                    {
                        IsGranted = true
                    }
                };
            //Action
            var isGranted = await _proxyPermissionChecker
                .IsGrantedAsync( 1, "perm", "testhost" );
            //Assert
            try
            {
                isGranted.ShouldBeTrue();
            }
            finally
            {
                _fakePermissionApi.IsGrantWithHttpInfo_Delegate 
                    = original_delegate;
            }
        }

        [Fact( DisplayName = "远程代理_未授权" )]
        public async Task IsGrantedAsync_Test2()
        {
            //Assert
            _proxyPermissionChecker.ShouldBeOfType
                <RemoteProxyPermissionChecker>();
            //Arrange
            var original_delegate = _fakePermissionApi
                .IsGrantWithHttpInfo_Delegate;
            _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                = input => new NormalizationResponse
                <cool.permission.client.Model.IsGrantOutput>
                {
                    Code = "00010100",
                    __normalization = true,
                    Data = new cool.permission.client.Model.IsGrantOutput
                    {
                        IsGranted = false
                    }
                };

            NormalizationException exception = default;
            try
            {
                //Action
                var isGranted = await _proxyPermissionChecker
                    .IsGrantedAsync( 1, "perm", "testhost" );
            }
            catch (NormalizationException ex)
            {
                exception = ex;
            }
            finally
            {
                _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                    = original_delegate;
            }
            //Assert
            exception.ShouldNotBeNull();
            exception.DetailCode.ShouldBe( "01" );
            exception.LevelCode.ShouldBe( "03" );
        }

        [Fact( DisplayName = "远程代理_授权服务返回非零错误级别" )]
        public async Task IsGrantedAsync_Test3()
        {
            //Assert
            _proxyPermissionChecker.ShouldBeOfType
                <RemoteProxyPermissionChecker>();
            //Arrange
            var original_delegate = _fakePermissionApi
                .IsGrantWithHttpInfo_Delegate;
            _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                = input => new NormalizationResponse
                <cool.permission.client.Model.IsGrantOutput>
                {
                    __normalization = true,
                    Code = "03010101",
                    Message = "授权服务返回非零错误级别"
                };


            NormalizationException exception = default;
            try
            {
                //Action
                var isGranted = await _proxyPermissionChecker
                    .IsGrantedAsync( 1, "perm", "testhost" );
            }
            catch (NormalizationException ex)
            {
                exception = ex;
            }
            finally
            {
                _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                    = original_delegate;
            }
            //Assert
            exception.ShouldNotBeNull();
            exception.DetailCode.ShouldBe( "02" );
            exception.LevelCode.ShouldBe( "03" );
        }

        [Fact( DisplayName = "远程代理_授权服务客户端异常" )]
        public async Task IsGrantedAsync_Test4()
        {
            //Assert
            _proxyPermissionChecker.ShouldBeOfType
                <RemoteProxyPermissionChecker>();
            //Arrange
            var original_delegate = _fakePermissionApi
                .IsGrantWithHttpInfo_Delegate;
            _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                = input => default;


            NormalizationException exception = default;
            try
            {
                //Action
                var isGranted = await _proxyPermissionChecker
                    .IsGrantedAsync( 1, "perm", "testhost" );
            }
            catch (NormalizationException ex)
            {
                exception = ex;
            }
            finally
            {
                _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                    = original_delegate;
            }
            //Assert
            exception.ShouldNotBeNull();
            exception.DetailCode.ShouldBe( "01" );
            exception.LevelCode.ShouldBe( "99" );
        }

        [Fact( DisplayName = "远程代理_授权服务返回空Data或客户端异常" )]
        public async Task IsGrantedAsync_Test5()
        {
            //Assert
            _proxyPermissionChecker.ShouldBeOfType
                <RemoteProxyPermissionChecker>();
            //Arrange
            var original_delegate = _fakePermissionApi
                .IsGrantWithHttpInfo_Delegate;
            _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                = input => new NormalizationResponse
                <cool.permission.client.Model.IsGrantOutput>
                {
                    __normalization = true,
                    Code = "00010100",
                    Data = null,
                };


            NormalizationException exception = default;
            try
            {
                //Action
                var isGranted = await _proxyPermissionChecker
                    .IsGrantedAsync( 1, "perm", "testhost" );
            }
            catch (NormalizationException ex)
            {
                exception = ex;
            }
            finally
            {
                _fakePermissionApi.IsGrantWithHttpInfo_Delegate
                    = original_delegate;
            }
            //Assert
            exception.ShouldNotBeNull();
            exception.DetailCode.ShouldBe( "03" );
            exception.LevelCode.ShouldBe( "03" );
        }
    }

}