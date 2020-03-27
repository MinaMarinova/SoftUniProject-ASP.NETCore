namespace FreelancePool.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using FreelancePool.Data;
    using FreelancePool.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext dbcontext;

        public HomeController(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            var category = this.dbcontext.Categories.FirstOrDefault();
            return this.View(category);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
