// ReSharper disable VirtualMemberCallInConstructor
using System.ComponentModel.DataAnnotations;

namespace FreelancePool.Data.Models
{
    public class CategoryUser
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}