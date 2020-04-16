namespace FreelancePool.Web.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using AutoMapper;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.ViewModels.Categories;

    public class ProjectViewModel : IMapFrom<Project>, IHaveCustomMappings
    {
        public ProjectViewModel()
        {
            this.CategoriesId = new HashSet<int>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription
        {
            get
            {
                var content = WebUtility.HtmlDecode(Regex.Replace(this.Description, @"<[^>]+>", string.Empty));
                return content.Length > 300
                        ? content.Substring(0, 300) + "..."
                        : content;
            }
        }

        public string AuthorPhotoUrl { get; set; }

        public string AuthorUserName { get; set; }

        public IEnumerable<int> CategoriesId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Project, ProjectViewModel>().ForMember(
                m => m.CategoriesId,
                opt => opt.MapFrom(x => x.ProjectCategories.Select(c => c.CategoryId)));
        }
    }
}
