namespace FreelancePool.Web.Controllers
{
    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ICategoriesService categoriesService;

        public HomeController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var categoriesViewModel = new AllCategoriesViewModel
            {
                Categories = this.categoriesService.GetAll<CategoryListViewModel>(),
            };

            return this.View(categoriesViewModel);
        }
    }
}
