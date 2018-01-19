using Abp.Web.Models;
using Cool.Normalization.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Cool.Normalization.Tests.Filters
{
    public class NormalizationExceptionFilter_Tests : NormalizationTestBase
    {
        [Fact]
        public void OnException_WrapOnError_False_Tests()
        {
            var normalizationExceptionFilter = Resolve<NormalizationExceptionFilter>();
            var testFactory = TestNormalizationExceptionWrapperFactory.Instance;
            normalizationExceptionFilter.NormalizationExceptionWrapperFactory = testFactory;
            var httpRequest = new TestHttpRequest();
            var httpResponse = new TestHttpResponse();
            var httpContext = new TestHttpContext( httpRequest ,httpResponse);
            httpRequest.SetHttpContext( httpContext );
            httpResponse.SetHttpContext(httpContext);
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            routeData.Values.TryAdd( "controller", "Test" );
            routeData.Values.TryAdd( "action", "Test" );
            var actionDescriptor = new ControllerActionDescriptor();
            actionDescriptor.ActionName = "Test";
            actionDescriptor.ControllerName = "Test";
            actionDescriptor.ControllerTypeInfo = typeof( TestController ).GetTypeInfo();
            actionDescriptor.MethodInfo = new TestController().GetTestMethodInfo();

            var actionContext = new ActionContext(httpContext,routeData, actionDescriptor);
            var errorContext = new ExceptionContext( actionContext, new List<IFilterMetadata>() { normalizationExceptionFilter } )
            {
                Exception = new TestException( "TestError" )
            };

            normalizationExceptionFilter.OnException( errorContext );

            testFactory.CallCountOfCreateFor.ShouldBeLessThan( 1 );
        }

        [Fact]
        public void OnException_WrapOnError_True_Tests()
        {
            var normalizationExceptionFilter = Resolve<NormalizationExceptionFilter>();
            var testFactory = TestNormalizationExceptionWrapperFactory.Instance;
            normalizationExceptionFilter.NormalizationExceptionWrapperFactory = testFactory;
            var httpRequest = new TestHttpRequest();
            var httpResponse = new TestHttpResponse();
            var httpContext = new TestHttpContext( httpRequest, httpResponse );
            httpRequest.SetHttpContext( httpContext );
            httpResponse.SetHttpContext( httpContext );
            var routeData = new Microsoft.AspNetCore.Routing.RouteData();
            routeData.Values.TryAdd( "controller", "Test" );
            routeData.Values.TryAdd( "action", "Test2" );
            var actionDescriptor = new ControllerActionDescriptor();
            actionDescriptor.ActionName = "Test2";
            actionDescriptor.ControllerName = "Test";
            actionDescriptor.ControllerTypeInfo = typeof( TestController ).GetTypeInfo();
            actionDescriptor.MethodInfo = new TestController().GetTestMethodInfo2();

            var actionContext = new ActionContext( httpContext, routeData, actionDescriptor );
            var errorContext = new ExceptionContext( actionContext, new List<IFilterMetadata>() { normalizationExceptionFilter } );
            errorContext.Exception = new TestException( "TestError" );

            normalizationExceptionFilter.OnException( errorContext );

            testFactory.CallCountOfCreateFor.ShouldBeGreaterThan( 0 );
        }


        [Code("55")]
        class TestController : ControllerBase
        {
            [WrapResult(true,false)]
            [Code( "66" )]
            public ObjectResult Test()
            {
                return new ObjectResult(new object());
            }

            [WrapResult( true, true )]
            [Code( "67" )]
            public ObjectResult Test2()
            {
                return new ObjectResult( new object() );
            }

            public MethodInfo GetTestMethodInfo()
            {
                return typeof( TestController ).GetMethod( nameof( Test ) );
            }
            public MethodInfo GetTestMethodInfo2()
            {
                return typeof( TestController ).GetMethod( nameof( Test2 ) );
            }
        }
        
        [Code("77")]
        class TestException : Exception
        {
            public TestException( string message ) : base( message )
            {
            }
        }
    }
}
