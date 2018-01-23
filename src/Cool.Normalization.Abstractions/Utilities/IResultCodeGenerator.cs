#if NET452
using System.Web.Mvc;
#else
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
#endif
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cool.Normalization.Utilities
{
    public interface IResultCodeGenerator
    {
        string GetCode( ActionDescriptor actionDescriptor, Type controllerType );

        string GetCode( ExceptionContext exceptionContext,Type controllerType );
    }
}
