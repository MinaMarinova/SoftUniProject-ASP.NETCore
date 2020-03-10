namespace FreelancePool.Data.Models
{
    using System.Collections.Generic;

    using FreelancePool.Data.Common.Models;

    public class Post : BaseDeletableModel<int>
    {
        public Post()
        {
            this.PostCategories = new HashSet<CategoryPost>();
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public virtual ICollection<CategoryPost> PostCategories { get; set; }
    }
}
