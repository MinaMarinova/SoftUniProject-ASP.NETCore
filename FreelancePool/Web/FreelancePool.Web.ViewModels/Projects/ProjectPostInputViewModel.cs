namespace FreelancePool.Web.ViewModels.Projects
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.Infrastructure.ValidationAttributes;
    using FreelancePool.Web.ViewModels.Categories;

    public class ProjectPostInputViewModel : IMapTo<Project>
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Display(Name = "Choose one or more categories")]
        [NotEmptyCollection(ErrorMessage ="You must select at least one category!")]
        public ICollection<int> CategoriesId { get; set; }

        public AllCategoriesViewModel AllCategories { get; set; }

        [Display(Name = "Enter emails of preferred freelancers to make them an offer")]
        public string UsersEmails { get; set; }
    }
}
