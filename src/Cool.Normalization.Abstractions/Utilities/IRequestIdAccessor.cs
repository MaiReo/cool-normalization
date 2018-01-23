#if NET452
using System.Web.Mvc;
#else
using Microsoft.AspNetCore.Mvc.Filters;
#endif

namespace Cool.Normalization.Utilities
{
    public interface IRequestIdAccessor
    {
        string RequestId { get; }
        string GetRequestId( ResultExecutingContext resultExecutingContext );
        string GetRequestId( ExceptionContext exceptionContext );
    }
}
