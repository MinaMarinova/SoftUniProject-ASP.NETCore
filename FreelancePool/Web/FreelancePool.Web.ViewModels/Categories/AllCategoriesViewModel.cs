namespace FreelancePool.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class AllCategoriesViewModel
    {
        public AllCategoriesViewModel()
        {
            this.Categories = new HashSet<CategoryListViewModel>();
        }

        public IEnumerable<CategoryListViewModel> Categories { get; set; }
    }
}
