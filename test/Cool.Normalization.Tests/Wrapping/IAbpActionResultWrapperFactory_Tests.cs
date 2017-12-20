using Abp.AspNetCore.Mvc.Results.Wrapping;
using Cool.Normalization.Models;
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
    public class IAbpActionResultWrapperFactory_Tests : MicroServicesnormalizationTestBase
    {
        private readonly IAbpActionResultWrapperFactory _abpActionResultWrapperFactory;

        public IAbpActionResultWrapperFactory_Tests()
        {
            _abpActionResultWrapperFactory = Resolve<IAbpActionResultWrapperFactory>();
        }

        [Fact]
        public void Should_Return_ObjectResultWrapper()
        {
            var value = new
            {
                TestClass = nameof( IAbpActionResultWrapperFactory_Tests ),
                TestMethod = nameof( Should_Return_ObjectResultWrapper )
            };
            var context = MakeResultExecutingContext( new ObjectResult( value ) );
            var wrapper = _abpActionResultWrapperFactory.CreateFor( context );
            wrapper.ShouldNotBeNull();
            wrapper.ShouldBeOfType<normalizationObjectActionResultWrapper>();
        }

        [Fact]
        public void Should_Return_JsonResultWrapper()
        {
            var value = new
            {
                TestClass = nameof( IAbpActionResultWrapperFactory_Tests ),
                TestMethod = nameof( Should_Return_ObjectResultWrapper )
            };
            var context = MakeResultExecutingContext( new JsonResult( value ) );
            var wrapper = _abpActionResultWrapperFactory.CreateFor( context );
            wrapper.ShouldNotBeNull();
            wrapper.ShouldBeOfType<normalizationJsonActionResultWrapper>();
        }

        [Fact]
        public void Should_Return_EmptyResultWrapper()
        {

            var context = MakeResultExecutingContext( new EmptyResult() );
            var wrapper = _abpActionResultWrapperFactory.CreateFor( context );
            wrapper.ShouldNotBeNull();
            wrapper.ShouldBeOfType<normalizationEmptyActionResultWrapper>();
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
            actionDescriptor.ControllerName = "Test";
            var actionContext = new ActionContext( httpContext, routeData, actionDescriptor );
            var actionExecutingContext = new ResultExecutingContext( actionContext, new List<IFilterMetadata>(), actionResult, null );
            return actionExecutingContext;
        }
    }
}
