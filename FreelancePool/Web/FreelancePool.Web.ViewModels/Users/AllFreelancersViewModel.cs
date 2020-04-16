namespace FreelancePool.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using FreelancePool.Web.ViewModels.Components;

    public class AllFreelancersViewModel
    {
        public AllFreelancersViewModel()
        {
            this.Freelancers = new HashSet<FreelancerViewModel>();
            this.TopFreelancers = new HashSet<FreelancerViewModel>();
            this.TopFreelancers = new HashSet<FreelancerViewModel>();
        }

        public string Order { get; set; }

        public IEnumerable<FreelancerViewModel> Freelancers { get; set; }

        public IEnumerable<FreelancerViewModel> TopFreelancers { get; set; }

        public IEnumerable<FreelancerViewModel> RecentlyJoined { get; set; }
    }
}
