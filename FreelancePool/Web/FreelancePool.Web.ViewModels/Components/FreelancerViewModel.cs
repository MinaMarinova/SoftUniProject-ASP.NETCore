namespace FreelancePool.Web.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class FreelancerViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public FreelancerViewModel()
        {
            this.Categories = new HashSet<string>();
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string PhotoUrl { get; set; }

        public int Stars { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, FreelancerViewModel>().ForMember(
                m => m.Categories,
                opt => opt.MapFrom(x => x.UserCategories.Select(c => c.Category.Name)));
        }
    }
}
