namespace FreelancePool.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using FreelancePool.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.CategoryUsers = new HashSet<CategoryUser>();
            this.CategoryProjects = new HashSet<CategoryProject>();
            this.CategoryPosts = new HashSet<CategoryPost>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string IconURL { get; set; }

        public virtual ICollection<CategoryUser> CategoryUsers { get; set; }

        public virtual ICollection<CategoryProject> CategoryProjects { get; set; }

        public virtual ICollection<CategoryPost> CategoryPosts { get; set; }
    }
}
