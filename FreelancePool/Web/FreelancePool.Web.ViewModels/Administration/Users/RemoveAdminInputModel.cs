namespace FreelancePool.Web.ViewModels.Administration.Users
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveAdminInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
