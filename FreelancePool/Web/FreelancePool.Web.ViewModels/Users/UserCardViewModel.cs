namespace FreelancePool.Web.ViewModels.Users
{
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class UserCardViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string EncryptedId { get; set; }

        public int Stars { get; set; }

        public string UserName { get; set; }

        public string PhotoUrl { get; set; }
    }
}
