// ReSharper disable VirtualMemberCallInConstructor
namespace FreelancePool.Data.Models
{
    using FreelancePool.Data.Common.Models;

    public class Recommendation : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public string RecipientId { get; set; }

        public ApplicationUser Recipient { get; set; }
    }
}