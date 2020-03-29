using AutoMapper;
using FreelancePool.Data.Models;
using FreelancePool.Services.Mapping;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FreelancePool.Web.ViewModels.Components
{
    public class RandomFreelancersViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public RandomFreelancersViewModel()
        {
            this.Categories = new HashSet<string>();
        }

        public string UserName { get; set; }

        public string PhotoUrl { get; set; }

        public int Stars { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, RandomFreelancersViewModel>().ForMember(
                m => m.Categories,
                opt => opt.MapFrom(x => x.UserCategories.Select(c => c.Category.Name)));
        }
    }
}
