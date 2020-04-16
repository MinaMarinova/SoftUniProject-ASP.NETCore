namespace FreelancePool.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using FreelancePool.Web.ViewModels.Components;

    public class CategoryDetailsViewModel
    {
        public CategoryDetailsViewModel()
        {
            this.Freelancers = new HashSet<FreelancerViewModel>();
            this.Projects = new HashSet<ProjectViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Order { get; set; }

        public IEnumerable<FreelancerViewModel> Freelancers { get; set; }

        public IEnumerable<ProjectViewModel> Projects { get; set; }
    }
}
