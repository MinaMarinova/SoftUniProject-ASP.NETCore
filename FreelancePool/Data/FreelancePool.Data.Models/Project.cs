namespace FreelancePool.Data.Models
{
    using System.Collections.Generic;

    using FreelancePool.Data.Common.Models;

    public class Project : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public ApplicationUser Author { get; set; }

        public ApplicationUser Executor { get; set; }

        public ProjectStatus Status { get; set; }

        public virtual ICollection<CategoryProject> ProjectCategories { get; set; }
    }
}
