using Abp.Runtime.Validation;
using Cool.Normalization.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Cool.Normalization.Tests
{
    public class ResultCodeGenerator_Tests : NormalizationTestBase
    {

        private readonly IResultCodeGenerator _resultCodeGenerator;

        public ResultCodeGenerator_Tests()
        {
            _resultCodeGenerator = Resolve<IResultCodeGenerator>();
        }

        [Fact]
        public void GetCode_Success_Test()
        {

            var actionDescriptor = new ControllerActionDescriptor();
            actionDescriptor.ActionName = "Test";
            actionDescriptor.ControllerName = "Test";
            actionDescriptor.ControllerTypeInfo = typeof( TestController ).GetTypeInfo();
            actionDescriptor.MethodInfo = new TestController().GetTestMethodInfo();

            var code = _resultCodeGenerator.GetCode( actionDescriptor, actionDescriptor.ControllerTypeInfo );
            code.ShouldBe( "00556600" );
        }

        [Fact]
        public void GetCode_Error_Test()
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
            actionDescriptor.ControllerTypeInfo = typeof( TestController ).GetTypeInfo();
            actionDescriptor.MethodInfo = new TestController().GetTestMethodInfo();

            var actionContext = new ActionContext( httpContext, routeData, actionDescriptor );
            var errorContext = new ExceptionContext( actionContext, new List<IFilterMetadata>() { } )
            {
                Exception = new TestException( "testerror" )
            };

            var code = _resultCodeGenerator.GetCode( errorContext, actionDescriptor.ControllerTypeInfo );
            code.ShouldBe( "77556647" );
        }

        [Fact]
        public void GetCode_Error_normalizationException_Test()
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
            actionDescriptor.ControllerTypeInfo = typeof( TestController ).GetTypeInfo();
            actionDescriptor.MethodInfo = new TestController().GetTestMethodInfo();

            var actionContext = new ActionContext( httpContext, routeData, actionDescriptor );
            var errorContext = new ExceptionContext( actionContext, new List<IFilterMetadata>() { } )
            {
                Exception = new NormalizationException( "47", "77", "testerror" )
            };

            var code = _resultCodeGenerator.GetCode( errorContext, actionDescriptor.ControllerTypeInfo );
            code.ShouldBe( "77556647" );
        }

        [Fact]
        public void GetCode_Error_AbpValidationException_Test()
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
            actionDescriptor.ControllerTypeInfo = typeof( TestController ).GetTypeInfo();
            actionDescriptor.MethodInfo = new TestController().GetTestMethodInfo();
            actionDescriptor.Parameters = new List<ParameterDescriptor>
            {
                new ParameterDescriptor()
                {
                     Name = "input",
                     ParameterType = typeof(TestController.TestInputDto)
                }
            };

            var actionContext = new ActionContext( httpContext, routeData, actionDescriptor );
            var errorContext = new ExceptionContext( actionContext, new List<IFilterMetadata>() { } )
            {
                Exception = new AbpValidationException( "errormsg" )
                {
                    ValidationErrors = new List<ValidationResult>
                     {
                         new ValidationResult( "validationError", new[] {nameof( TestController .TestInputDto.Property) } )
                     }
                }
            };

            var code = _resultCodeGenerator.GetCode( errorContext, actionDescriptor.ControllerTypeInfo );
            code.ShouldBe( "01556645" );
        }

        [Code( "55" )]
        class TestController : ControllerBase
        {
            [Code( "66" )]
            public ObjectResult Test( TestInputDto input )
            {
                return new ObjectResult( new object() );
            }

            public MethodInfo GetTestMethodInfo()
            {
                return typeof( TestController ).GetMethod( nameof( Test ) );
            }

            [Code( "01" )]
            public class TestInputDto
            {
                [Code( "45" )]
                [Required]
                public string Property { get; set; }
            }
        }
        [Code( "47" )]
        [Code( "77", CodePart = CodePart.Level )]
        class TestException : Exception
        {
            public TestException( string message ) : base( message )
            {
            }
        }

    }
}
