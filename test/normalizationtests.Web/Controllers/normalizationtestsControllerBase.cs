using Abp.AspNetCore.Mvc.Controllers;

namespace normalizationtests.Web.Controllers
{
    public abstract class normalizationtestsControllerBase: AbpController
    {
        protected normalizationtestsControllerBase()
        {
            LocalizationSourceName = normalizationtestsConsts.LocalizationSourceName;
        }
    }
}