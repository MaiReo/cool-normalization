using Cool.Normalization.Configuration;
using Cool.Normalization.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class RequestIdAccessor_Tests : NormalizationTestBase
    {

        private readonly IRequestIdAccessor _requestIdAccessor;

        private readonly INormalizationConfiguration _normalizationConfiguration;

        public RequestIdAccessor_Tests()
        {
            this._requestIdAccessor = Resolve<IRequestIdAccessor>();
            this._normalizationConfiguration = Resolve<INormalizationConfiguration>();
        }

        [Fact( DisplayName = "输出包装模块直接获取请求Id" )]
        public void RequestId_Test()
        {
            const string const_request_id = "a request id";
            _requestIdAccessor.RequestId.ShouldBeNullOrEmpty();
            TestHttpContext.Current.Request.Headers[_normalizationConfiguration.RequestIdHeaderName] = const_request_id;
            _requestIdAccessor.RequestId.ShouldBe( const_request_id );
            TestHttpContext.Current.Request.Headers.Remove( _normalizationConfiguration.RequestIdHeaderName );
        }

        [Fact( DisplayName = "输出包装模块从异常获取请求Id" )]
        public void GetRequestId_Exception_Test()
        {
            const string const_request_id = "it is a request id";
            var httpRequest = new TestHttpRequest();
            httpRequest.Headers[_normalizationConfiguration.RequestIdHeaderName] = const_request_id;
            var httpResponse = new TestHttpResponse();
            var httpContext = new TestHttpContext( httpRequest, httpResponse );
            httpRequest.SetHttpContext( httpContext );
            httpResponse.SetHttpContext( httpContext );
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            routeData.Values.TryAdd( "controller", "Test" );
            routeData.Values.TryAdd( "action", "Test" );
            var actionDescriptor = new ControllerActionDescriptor();
            actionDescriptor.ActionName = "Test";
            actionDescriptor.ControllerName = "Test";
            var actionContext = new ActionContext( httpContext, routeData, actionDescriptor );
            var errorContext = new ExceptionContext( actionContext, new List<IFilterMetadata>() { } )
            {
                Exception = new Exception( "testerror" )
            };

            var requestId = _requestIdAccessor.GetRequestId( errorContext );

            requestId.ShouldBe( const_request_id );
        }

        [Fact( DisplayName = "输出包装模块从结果获取请求Id" )]
        public void GetRequestId_Success_Test()
        {
            const string const_request_id = "it is not a true request id";
            var httpRequest = new TestHttpRequest();
            httpRequest.Headers[_normalizationConfiguration.RequestIdHeaderName] = const_request_id;
            var httpResponse = new TestHttpResponse();
            var httpContext = new TestHttpContext( httpRequest, httpResponse );
            httpRequest.SetHttpContext( httpContext );
            httpResponse.SetHttpContext( httpContext );
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            routeData.Values.TryAdd( "controller", "Test" );
            routeData.Values.TryAdd( "action", "Test" );
            var actionDescriptor = new ControllerActionDescriptor();
            actionDescriptor.ActionName = "Test";
            actionDescriptor.ControllerName = "Test";
            var actionContext = new ActionContext( httpContext, routeData, actionDescriptor );
            var actionExecutingContext = new ResultExecutingContext( actionContext, new List<IFilterMetadata>(), new EmptyResult(), null );

            var requestId = _requestIdAccessor.GetRequestId( actionExecutingContext );

            requestId.ShouldBe( const_request_id );
        }
    }
}
