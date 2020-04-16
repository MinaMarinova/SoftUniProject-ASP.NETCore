namespace FreelancePool.Web.ViewModels.Messages
{
    using System.ComponentModel.DataAnnotations;

    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.ViewModels.Users;

    public class MessageViewModel : IMapTo<Message>, IMapFrom<Message>
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        public UserCardViewModel Author { get; set; }

        public int ProjectId { get; set; }
    }
}
