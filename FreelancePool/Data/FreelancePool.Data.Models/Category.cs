namespace FreelancePool.Data.Models
{
    using System;
    using System.Collections.Generic;

    using FreelancePool.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.CategoryUsers = new HashSet<CategoryUser>();
            this.CategoryProjects = new HashSet<CategoryProject>();
            this.CategoryPosts = new HashSet<CategoryPost>();
        }

        public string Name { get; set; }

        public virtual ICollection<CategoryUser> CategoryUsers { get; set; }

        public virtual ICollection<CategoryProject> CategoryProjects { get; set; }

        public virtual ICollection<CategoryPost> CategoryPosts { get; set; }
    }
}
