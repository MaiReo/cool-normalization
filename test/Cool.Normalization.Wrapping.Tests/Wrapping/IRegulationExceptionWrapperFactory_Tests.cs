using Cool.Normalization.Filters;
using Cool.Normalization.Wrapping;
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
    public class InormalizationExceptionWrapperFactory_Tests : NormalizationTestBase
    {
        public readonly INormalizationExceptionWrapperFactory _normalizationExceptionWrapperFactory;

        public readonly IExceptionFilter _exceptionFilter;
        public InormalizationExceptionWrapperFactory_Tests()
        {
            _normalizationExceptionWrapperFactory = Resolve<INormalizationExceptionWrapperFactory>();
            _exceptionFilter = Resolve<IExceptionFilter>();
        }

        [Fact( DisplayName = "输出包装模块异常包装工厂生成异常包装者" )]
        public void Should_Return_ExceptionResultWrapper()
        {
            var errorContext = CreateContext();
            var wrapper = _normalizationExceptionWrapperFactory.CreateFor( errorContext );
            wrapper.ShouldNotBeNull();
            wrapper.ShouldBeOfType<NormalizationExceptionWrapper>();
        }

        [Fact( DisplayName = "输出包装模块异常包装工厂依赖注入检测" )]
        public void IExceptionFilter_ShouldBe_NormalizationExceptionFilter()
        {
            _exceptionFilter.ShouldBeOfType<NormalizationExceptionFilter>();
        }

        private ExceptionContext CreateContext()
        {
            var httpRequest = new TestHttpRequest();
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
            var errorContext = new ExceptionContext( actionContext, new List<IFilterMetadata>() )
            {
                Exception = new Exception( "TestError" )
            };
            return errorContext;
        }

    }
}
