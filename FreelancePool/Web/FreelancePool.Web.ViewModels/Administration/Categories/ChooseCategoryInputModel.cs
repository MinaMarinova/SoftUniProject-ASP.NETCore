namespace FreelancePool.Web.ViewModels.Administration.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class ChooseCategoryInputModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
