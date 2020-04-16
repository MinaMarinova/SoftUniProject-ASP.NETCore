namespace FreelancePool.Web.ViewModels.Categories
{
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class CategoryListViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IconURL { get; set; }
    }
}
