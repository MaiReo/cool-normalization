using Abp.Events.Bus;
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
    public class normalizationExceptionWrapper_Tests : MicroServicesnormalizationTestBase
    {
        private readonly IResultAuditingHelper _resultAuditingHelper;
        private readonly IRequestIdAccessor _requestIdAccessor;
        private readonly IResultCodeGenerator _resultCodeGenerator;
        private readonly IErrorMessageGenerator _errorMessageGenerator;
        private readonly IEventBus _eventBus;
        public normalizationExceptionWrapper_Tests()
        {
            _resultAuditingHelper = Resolve<IResultAuditingHelper>();
            _requestIdAccessor = Resolve<IRequestIdAccessor>();
            _resultCodeGenerator = Resolve<IResultCodeGenerator>();
            _errorMessageGenerator = Resolve<IErrorMessageGenerator>();
            _eventBus = NullEventBus.Instance;
        }

        [Fact]
        public void Wrap_Test()
        {
            var wrapper = new normalizationExceptionWrapper( _eventBus, _requestIdAccessor, _resultCodeGenerator, _errorMessageGenerator, _resultAuditingHelper );
            var context = MakeExceptionContext( new NormalizationException( "04", "76", "Error" ) );

            wrapper.Wrap( context );

            context.Result.ShouldNotBeNull();
            (context.Result as ObjectResult).Value.ShouldBeAssignableTo<NormalizationResponseBase>();
            ((NormalizationResponseBase)((ObjectResult)context.Result).Value).Code.ShouldBe( "76123404" );
            ((NormalizationResponseBase)((ObjectResult)context.Result).Value).Message.ShouldBe( "Error" );
        }

        private ExceptionContext MakeExceptionContext( Exception exception )
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
            var exceptionContext = new ExceptionContext( actionContext, new List<IFilterMetadata>() )
            {
                Exception = exception
            };
            return exceptionContext;
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
