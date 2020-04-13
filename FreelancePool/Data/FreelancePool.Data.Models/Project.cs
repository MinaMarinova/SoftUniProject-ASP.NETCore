namespace FreelancePool.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FreelancePool.Data.Common.Models;

    public class Project : BaseDeletableModel<int>
    {
        public Project()
        {
            this.AppliedUsers = new HashSet<UserCandidateProject>();
            this.ProjectCategories = new HashSet<CategoryProject>();
            this.SuggestedUsers = new HashSet<ProjectOfferUser>();
            this.MessagesLeft = new HashSet<Message>();
        }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000)]
        public string Description { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string ExecutorId { get; set; }

        public virtual ApplicationUser Executor { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }

        public virtual ICollection<Message> MessagesLeft { get; set; }

        public virtual ICollection<CategoryProject> ProjectCategories { get; set; }

        public virtual ICollection<ProjectOfferUser> SuggestedUsers { get; set; }

        public virtual ICollection<UserCandidateProject> AppliedUsers { get; set; }
    }
}
