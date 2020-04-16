namespace FreelancePool.Web.ViewModels.Projects
{
    using System.Collections.Generic;
    using AutoMapper;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using FreelancePool.Web.ViewModels.Categories;
    using FreelancePool.Web.ViewModels.Components;

    public class AllProjectsViewModel
    {
        public AllProjectsViewModel()
        {
            this.AllProjects = new HashSet<ProjectViewModel>();
            this.MostWanted = new HashSet<ProjectTitleAndApplicantsViewModel>();
            this.SelectedCategoriesId = new HashSet<int>();
        }

        public IEnumerable<ProjectViewModel> AllProjects { get; set; }

        public IEnumerable<ProjectTitleAndApplicantsViewModel> MostWanted { get; set; }


        public ICollection<int> SelectedCategoriesId { get; set; }

        public AllCategoriesViewModel AllCategories { get; set; }
    }
}
