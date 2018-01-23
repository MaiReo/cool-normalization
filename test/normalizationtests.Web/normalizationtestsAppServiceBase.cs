using Abp.Application.Services;

namespace normalizationtests
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class normalizationtestsAppServiceBase : ApplicationService
    {
        protected normalizationtestsAppServiceBase()
        {
            LocalizationSourceName = normalizationtestsConsts.LocalizationSourceName;
        }
    }
}