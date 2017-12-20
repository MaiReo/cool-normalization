using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cool.Normalization.Wrapping
{
    public interface INormalizationExceptionWrapper
    {
        void Wrap(ExceptionContext context);
    }
}
