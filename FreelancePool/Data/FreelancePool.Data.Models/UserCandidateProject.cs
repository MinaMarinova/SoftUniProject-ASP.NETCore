using System.ComponentModel.DataAnnotations;

namespace FreelancePool.Data.Models
{
    public class UserCandidateProject
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
