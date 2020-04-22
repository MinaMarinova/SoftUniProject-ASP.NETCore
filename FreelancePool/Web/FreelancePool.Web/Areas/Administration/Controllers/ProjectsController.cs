namespace FreelancePool.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Administration.Projects;
    using Microsoft.AspNetCore.Mvc;

    public class ProjectsController : AdministrationController
    {
        private const string DeleteSuccessMessage = "You have successfully deleted the project: {0}";
        private const string DeleteErrorMessage = "Failed to delete the project: {0}";

        private readonly IProjectsService projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            this.projectsService = projectsService;
        }

        public IActionResult Delete()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteProjectInputModel inputModel)
        {
            try
            {
                if (await this.projectsService.DeleteAsync(inputModel.Title, inputModel.AuthorEmail) > 0)
                {
                    this.TempData["Success"] = string.Format(DeleteSuccessMessage, inputModel.Title);
                }
                else
                {
                    this.TempData["Error"] = string.Format(DeleteErrorMessage, inputModel.Title);
                }
            }
            catch (ArgumentNullException ex)
            {
                this.TempData["Error"] = ex.ParamName;
            }

            return this.RedirectToAction(nameof(this.Delete));
        }
    }
}
