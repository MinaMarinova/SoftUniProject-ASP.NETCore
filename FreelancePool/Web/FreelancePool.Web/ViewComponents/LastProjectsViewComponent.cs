namespace FreelancePool.Web.ViewComponents
{
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Components;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class LastProjectsViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProjectsService projectsService;

        public LastProjectsViewComponent(UserManager<ApplicationUser> userManager, IProjectsService projectsService)
        {
            this.userManager = userManager;
            this.projectsService = projectsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            var lastProjects = this.projectsService.GetLastProjects<ProjectViewModel>(user);

            return this.View(lastProjects);
        }
    }
}
