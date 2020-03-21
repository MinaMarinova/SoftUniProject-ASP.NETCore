namespace FreelancePool.Data.Models
{
    public class CategoryProject
    {
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}