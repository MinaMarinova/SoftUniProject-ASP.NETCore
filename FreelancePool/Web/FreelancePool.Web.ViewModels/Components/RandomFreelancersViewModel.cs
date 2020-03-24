using FreelancePool.Data.Models;
using FreelancePool.Services.Mapping;

namespace FreelancePool.Web.ViewModels.Components
{
    public class RandomFreelancersViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public string PhotoUrl { get; set; }

        public int Stars { get; set; }
    }
}
