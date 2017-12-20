using Microsoft.AspNetCore.Mvc.Filters;

namespace Cool.Normalization.Wrapping
{
    public class NullnormalizationExceptionWrapper : INormalizationExceptionWrapper
    {
       
        public NullnormalizationExceptionWrapper()
        {
            
        }


        public void Wrap( ExceptionContext context )
        {
            //No action.
        }
        
    }
}
