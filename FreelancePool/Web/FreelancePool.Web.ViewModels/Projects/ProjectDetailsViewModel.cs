namespace FreelancePool.Web.ViewModels.Projects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.ViewModels.Messages;
    using FreelancePool.Web.ViewModels.Users;
    using Ganss.XSS;

    public class ProjectDetailsViewModel : IMapFrom<Project>, IHaveCustomMappings
    {
        public ProjectDetailsViewModel()
        {
            this.Candidates = new HashSet<UserCardViewModel>();
            this.Suggested = new HashSet<UserCardViewModel>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public DateTime CreatedOn { get; set; }

        public ProjectStatus Status { get; set; }

        public string AuthorId { get; set; }

        public string AuthorPhotoUrl { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorEmail { get; set; }

        public MessageViewModel Message { get; set; }

        public IEnumerable<MessageViewModel> MessagesLeft { get; set; }

        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<UserCardViewModel> Suggested { get; set; }

        public IEnumerable<UserCardViewModel> Candidates { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Project, ProjectDetailsViewModel>().ForMember(
                m => m.Categories,
                opt => opt.MapFrom(x => x.ProjectCategories.Select(c => c.Category.Name)));

            configuration.CreateMap<Project, ProjectDetailsViewModel>().ForMember(
                m => m.Suggested,
                opt => opt.MapFrom(x => x.SuggestedUsers.Select(su => su.User)))
                .ForMember(
                m => m.Candidates,
                opt => opt.MapFrom(x => x.AppliedUsers.Select(au => au.User)));
        }
    }
}
