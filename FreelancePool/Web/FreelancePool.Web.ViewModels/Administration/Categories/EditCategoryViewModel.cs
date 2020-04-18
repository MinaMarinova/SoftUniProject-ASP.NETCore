namespace FreelancePool.Web.ViewModels.Administration.Categories
{
    using System.ComponentModel.DataAnnotations;

    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class EditCategoryViewModel : IMapTo<Category>, IMapFrom<Category>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string CurrentName { get; set; }
    }
}
