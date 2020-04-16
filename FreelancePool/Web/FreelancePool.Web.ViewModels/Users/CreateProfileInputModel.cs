namespace FreelancePool.Web.ViewModels.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.Infrastructure.ValidationAttributes;
    using FreelancePool.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class CreateProfileInputModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Change username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailUnique", controller: "Users")]
        [Display(Name = "Change email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public string PhotoUrl { get; set; }

        [Display(Name = "Change photo")]
        public IFormFile NewPhoto { get; set; }

        [Required]
        [StringLength(10000, MinimumLength = 20)]
        public string Summary { get; set; }

        [Display(Name = "Choose one or more categories")]
        [NotEmptyCollection(ErrorMessage = "You must select at least one category!")]
        public ICollection<int> CategoriesId { get; set; }

        public AllCategoriesViewModel AllCategories { get; set; }
    }
}
