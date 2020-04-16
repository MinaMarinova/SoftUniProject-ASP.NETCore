namespace FreelancePool.Web.ViewModels.Projects
{
    using AutoMapper;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class ProjectTitleAndApplicantsViewModel : IMapFrom<Project>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ApplicantsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Project, ProjectTitleAndApplicantsViewModel>().ForMember(
                m => m.ApplicantsCount,
                opt => opt.MapFrom(x => x.AppliedUsers.Count));
        }
    }
}
