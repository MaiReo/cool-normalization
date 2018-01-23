using Microsoft.AspNetCore.Mvc.Filters;

namespace Cool.Normalization.Wrapping
{
    public class NullNormalizationExceptionWrapper : INormalizationExceptionWrapper
    {
       
        public NullNormalizationExceptionWrapper()
        {
            
        }


        public void Wrap( ExceptionContext context )
        {
            //No action.
        }
        
    }
}
