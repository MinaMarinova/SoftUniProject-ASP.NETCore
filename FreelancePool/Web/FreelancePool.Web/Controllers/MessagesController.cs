namespace FreelancePool.Web.Controllers
{
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Projects;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.FreelancerRoleName)]
    public class MessagesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMessagesService messagesService;

        public MessagesController(
            UserManager<ApplicationUser> userManager,
            IMessagesService messagesService)
        {
            this.userManager = userManager;
            this.messagesService = messagesService;
        }

        [HttpPost]
        public async Task<IActionResult> Write(ProjectDetailsViewModel inputView)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction("DetailsById", "Projects", new { id = inputView.Message.ProjectId });
            }

            var authorId = this.userManager.GetUserId(this.User);

            await this.messagesService.Create(inputView.Message.Content, inputView.Message.ProjectId, authorId);

            return this.RedirectToAction("DetailsById", "Projects", new { id = inputView.Message.ProjectId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var projectId = await this.messagesService.Delete(id);

            return this.RedirectToAction("DetailsById", "Projects", new { id = projectId });
        }
    }
}
