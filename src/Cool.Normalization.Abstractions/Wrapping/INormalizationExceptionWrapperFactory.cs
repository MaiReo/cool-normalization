using Microsoft.AspNetCore.Mvc.Filters;

namespace Cool.Normalization.Wrapping
{
    public interface INormalizationExceptionWrapperFactory
    {
        INormalizationExceptionWrapper CreateFor( ExceptionContext exceptionContext );
    }
}
