namespace FreelancePool.Web.ViewComponents
{
    using System.Linq;

    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesViewComponent(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IViewComponentResult Invoke()
        {
            var allCategories = new AllCategoriesViewModel
            {
                Categories = this.categoriesService.GetAll<CategoryListViewModel>().OrderBy(c => c.Name),
            };

            return this.View(allCategories);
        }
    }
}
