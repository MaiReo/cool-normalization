using Cool.Normalization.Auditing;
using Cool.Normalization.Models;
using Cool.Normalization.Utilities;
using Cool.Normalization.Wrapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Shouldly;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class normalizationJsonActionResultWrapper_Tests : MicroServicesnormalizationTestBase
    {
        private readonly IResultAuditingHelper _resultAuditingHelper;
        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly IResultCodeGenerator _resultCodeGenerator;

        public normalizationJsonActionResultWrapper_Tests()
        {
            _resultAuditingHelper = Resolve<IResultAuditingHelper>();
            _requestIdAccessor = Resolve<IRequestIdAccessor>();
            _resultCodeGenerator = Resolve<IResultCodeGenerator>();
        }

        [Fact]
        public void Wrap_Test()
        {
            var wrapper = new normalizationJsonActionResultWrapper( _resultAuditingHelper, _requestIdAccessor, _resultCodeGenerator );
            var result = new JsonResult( new { name = "value" } );
            var context = MakeResultExecutingContext( result );

            wrapper.Wrap( context );

            result.Value.ShouldNotBeNull();
            result.Value.ShouldBeAssignableTo<NormalizationResponseBase>();
        }

        private ResultExecutingContext MakeResultExecutingContext( IActionResult actionResult )
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
            actionDescriptor.MethodInfo = typeof( TestController ).GetMethod( nameof( TestController.TestMethod ) );
            actionDescriptor.ControllerName = "Test";
            actionDescriptor.ControllerTypeInfo = typeof( TestController ).GetTypeInfo();
            var actionContext = new ActionContext( httpContext, routeData, actionDescriptor );
            var actionExecutingContext = new ResultExecutingContext( actionContext, new List<IFilterMetadata>(), actionResult, new TestController() );

            return actionExecutingContext;
        }

        [Code( "12" )]
        class TestController : Controller
        {
            [Code( "34" )]
            public string TestMethod()
            {
                return "hey";
            }
        }
    }
}
