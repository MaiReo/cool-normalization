#if NET452
using System.Web.Mvc;
#else
using Microsoft.AspNetCore.Mvc.Filters;
#endif

namespace Cool.Normalization.Wrapping
{
    public interface INormalizationExceptionWrapper
    {
        void Wrap(ExceptionContext context);
    }
}
