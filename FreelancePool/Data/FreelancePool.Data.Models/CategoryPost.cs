namespace FreelancePool.Data.Models
{
    public class CategoryPost
    {
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}