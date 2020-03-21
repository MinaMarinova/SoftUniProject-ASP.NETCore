// ReSharper disable VirtualMemberCallInConstructor
namespace FreelancePool.Data.Models
{
    using FreelancePool.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;

    public class Recommendation : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public virtual ApplicationUser Recipient { get; set; }
    }
}