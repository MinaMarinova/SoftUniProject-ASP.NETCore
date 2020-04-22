namespace FreelancePool.Web.Areas.Administration.Controllers
{
    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Administration.Dashboard;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        public DashboardController()
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
