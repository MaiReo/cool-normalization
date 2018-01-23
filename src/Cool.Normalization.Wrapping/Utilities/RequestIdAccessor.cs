using Abp.Dependency;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Cool.Normalization.Configuration;

namespace Cool.Normalization.Utilities
{
    internal class RequestIdAccessor : IRequestIdAccessor, ISingletonDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly INormalizationConfiguration _normalizationConfiguration;


        public RequestIdAccessor( IHttpContextAccessor httpContextAccessor ,
            INormalizationConfiguration normalizationConfiguration)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._normalizationConfiguration = normalizationConfiguration;
        }
        public string RequestId => GetRequestId();

        private string GetRequestId() => _httpContextAccessor?.HttpContext?.Request?.Headers[_normalizationConfiguration.RequestIdHeaderName].FirstOrDefault();

        public string GetRequestId( ResultExecutingContext resultExecutingContext )
        {
            return (resultExecutingContext?.HttpContext ?? _httpContextAccessor?.HttpContext)?.Request?.Headers[_normalizationConfiguration.RequestIdHeaderName].FirstOrDefault();
        }

        public string GetRequestId( ExceptionContext exceptionContext )
        {
            return (exceptionContext?.HttpContext ?? _httpContextAccessor?.HttpContext)?.Request?.Headers[_normalizationConfiguration.RequestIdHeaderName].FirstOrDefault();
        }
    }
}
