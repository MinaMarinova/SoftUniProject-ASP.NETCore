namespace FreelancePool.Web.ViewModels.Administration.Projects
{
    using System.ComponentModel.DataAnnotations;

    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class DeleteProjectInputModel : IMapFrom<Project>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Author's email")]
        public string AuthorEmail { get; set; }
    }
}
