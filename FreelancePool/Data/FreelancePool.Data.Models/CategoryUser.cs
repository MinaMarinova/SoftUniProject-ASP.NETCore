// ReSharper disable VirtualMemberCallInConstructor
using System.ComponentModel.DataAnnotations;

namespace FreelancePool.Data.Models
{
    public class CategoryUser
    {
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}