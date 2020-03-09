namespace FreelancePool.Data.Models
{
    using FreelancePool.Data.Common.Models;
    using System;

    public class Category : BaseDeletableModel<string>
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
    }
}
