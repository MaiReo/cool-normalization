using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace normalizationtests.Web.Controllers
{
    public class HomeController : normalizationtestsControllerBase
    {
        public IActionResult Index()
        => Redirect( "~/swagger" );
    }
}