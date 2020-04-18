using FreelancePool.Data.Models;
using FreelancePool.Services.Mapping;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FreelancePool.Web.ViewModels.Administration.Categories
{
    public class CreateCategoryInputModel : IMapTo<Category>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Select icon")]
        public IFormFile Icon { get; set; }
    }
}
