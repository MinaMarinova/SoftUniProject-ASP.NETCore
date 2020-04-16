namespace FreelancePool.Web.ViewModels.Users
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.ViewModels.Projects;
    using FreelancePool.Web.ViewModels.Recommendations;
    using Ganss.XSS;

    public class DetailsViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public DetailsViewModel()
        {
            this.ProjectsPosted = new HashSet<ProjectTitleAndAuthorViewModel>();
            this.ProjectsApplied = new HashSet<ProjectTitleAndAuthorViewModel>();
            this.ProjectsOffered = new HashSet<ProjectTitleAndAuthorViewModel>();
            this.ProjectsCompleted = new HashSet<ProjectTitleAndAuthorViewModel>();
            this.Recommendations = new HashSet<RecommendationViewModel>();
        }

        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhotoUrl { get; set; }

        public string Summary { get; set; }

        public string SanitizedSummary => new HtmlSanitizer().Sanitize(this.Summary);

        public string PhoneNumber { get; set; }

        public int Stars { get; set; }

        public IEnumerable<RecommendationViewModel> Recommendations { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<ProjectTitleAndAuthorViewModel> ProjectsPosted { get; set; }

        public IEnumerable<ProjectTitleAndAuthorViewModel> ProjectsApplied { get; set; }

        public IEnumerable<ProjectTitleAndAuthorViewModel> ProjectsOffered { get; set; }

        public IEnumerable<ProjectTitleAndAuthorViewModel> ProjectsCompleted { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, DetailsViewModel>().ForMember(
                m => m.Categories,
                opt => opt.MapFrom(x => x.UserCategories.Select(c => c.Category.Name)))
            .ForMember(
                m => m.ProjectsApplied, opt => opt.MapFrom(u => u.ProjectsApplied.Select(up => up.Project)))
            .ForMember(
                m => m.ProjectsOffered, opt => opt.MapFrom(u => u.ProjectsOffered.Select(up => up.Project)));
        }
    }
}
