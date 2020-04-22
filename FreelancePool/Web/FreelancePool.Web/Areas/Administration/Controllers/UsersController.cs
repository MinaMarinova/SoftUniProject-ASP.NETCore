namespace FreelancePool.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.Filters;
    using FreelancePool.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AdministrationController
    {
        private const string RegisterAdminSeccessMessage = "You have successfully registered a new administrator: {0}!";
        private const string RegisterAdminErrorMessage = "The registration failed!";
        private const string RemoveSuccessMessage = "You have successfully removed the {0} {1}!";
        private const string RemoveErrorMessage = "There is no {0} with email {1}!";

        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthorizeRootUserFilterAttribute))]
        public IActionResult RegisterAdmin()
        {
            return this.View();
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizeRootUserFilterAttribute))]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            try
            {
                if (!await this.usersService.AddAdmin(inputModel.UserName, inputModel.Email, inputModel.Password))
                {
                    this.TempData["Error"] = RegisterAdminErrorMessage;
                    return this.View(inputModel);
                }

                this.TempData["Success"] = string.Format(RegisterAdminSeccessMessage, inputModel.UserName);
            }
            catch (ArgumentException ex)
            {
                this.TempData["Error"] = ex.Message;
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        [ServiceFilter(typeof(AuthorizeRootUserFilterAttribute))]
        public IActionResult RemoveAdmin()
        {
            return this.View();
        }

        [HttpPost]
        [ServiceFilter(typeof(AuthorizeRootUserFilterAttribute))]
        public async Task<IActionResult> RemoveAdmin(RemoveUserInputModel inputModel)
        {
            try
            {
                string adminName = await this.usersService.RemoveAdminAsync(inputModel.Email, GlobalConstants.AdministratorRoleName);
                this.TempData["Success"] = string.Format(RemoveSuccessMessage, GlobalConstants.AdministratorRoleName, adminName);
            }
            catch
            {
                this.TempData["Error"] = string.Format(RemoveErrorMessage, GlobalConstants.AdministratorRoleName, inputModel.Email);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult RemoveFreelancer()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFreelancer(RemoveUserInputModel inputModel)
        {
            try
            {
                string freelancerName = await this.usersService.RemoveAdminAsync(inputModel.Email, GlobalConstants.FreelancerRoleName);
                this.TempData["Success"] = string.Format(RemoveSuccessMessage, GlobalConstants.FreelancerRoleName, freelancerName);
            }
            catch
            {
                this.TempData["Error"] = string.Format(RemoveSuccessMessage, GlobalConstants.FreelancerRoleName, inputModel.Email);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult RemoveUser()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(RemoveUserInputModel inputModel)
        {
            try
            {
                string freelancerName = await this.usersService.RemoveUserAsync(inputModel.Email);
                this.TempData["Success"] = string.Format(RemoveSuccessMessage, "user", freelancerName);
            }
            catch
            {
                this.TempData["Error"] = string.Format(RemoveErrorMessage, "user", inputModel.Email);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
