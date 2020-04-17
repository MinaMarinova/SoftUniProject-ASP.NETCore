namespace FreelancePool.Web.ViewModels.Administration.Users
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveUserInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
