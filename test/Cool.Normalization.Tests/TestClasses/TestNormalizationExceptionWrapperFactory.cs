using Cool.Normalization.Wrapping;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cool.Normalization.Tests
{
    internal class TestNormalizationExceptionWrapperFactory : INormalizationExceptionWrapperFactory
    {
        public static TestNormalizationExceptionWrapperFactory Instance => new TestNormalizationExceptionWrapperFactory();

        public int CallCountOfCreateFor { get; set; }

        public INormalizationExceptionWrapper CreateFor( ExceptionContext exceptionContext )
        {
            CallCountOfCreateFor++;
            return new NullnormalizationExceptionWrapper();
        }
    }
}