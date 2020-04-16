using FreelancePool.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FreelancePool.Web.Areas.Administration.Controllers
{
    public class UsersController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.View();
        }

        [ServiceFilter(typeof(AuthorizeRootUserFilterAttribute))]
        public IActionResult RegisterAdmin()
        {
            return this.View();
        }
    }
}
