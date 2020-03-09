// ReSharper disable VirtualMemberCallInConstructor
namespace FreelancePool.Data.Models
{
    public class CategoryUser
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}