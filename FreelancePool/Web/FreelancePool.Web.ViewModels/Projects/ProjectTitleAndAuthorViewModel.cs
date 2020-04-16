namespace FreelancePool.Web.ViewModels.Projects
{
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class ProjectTitleAndAuthorViewModel : IMapFrom<Project>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorPhotoUrl { get; set; }
    }
}
