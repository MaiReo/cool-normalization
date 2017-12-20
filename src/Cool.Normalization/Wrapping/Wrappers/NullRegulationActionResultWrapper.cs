using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Dependency;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cool.Normalization.Wrapping
{
    public class NullnormalizationActionResultWrapper : IAbpActionResultWrapper
    {
       

        public NullnormalizationActionResultWrapper()
        {
           
        }

        public void Wrap( ResultExecutingContext actionResult )
        {
            //No action.
        }
    }
}