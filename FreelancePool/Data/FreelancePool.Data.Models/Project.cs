namespace FreelancePool.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using FreelancePool.Data.Common.Models;

    public class Project : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public string ExecutorId { get; set; }

        public ApplicationUser Executor { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }

        public virtual ICollection<CategoryProject> ProjectCategories { get; set; }
    }
}
