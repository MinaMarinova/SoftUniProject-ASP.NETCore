namespace FreelancePool.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using FreelancePool.Services.Data;
    using FreelancePool.Web.Filters;
    using FreelancePool.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AdministrationController
    {
        private const string RegisterAdminSeccessMessage = "You have successfully registered a new administrator: {0}!";
        private const string RegisterAdminErrorMessage = "The registration failed!";
        private const string RemoveSuccessMessage = "You have successfully removed the administrator {0}!";
        private const string RemoveErrorMessage = "There is no administrator with such an email!";

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

            if (!await this.usersService.AddAdmin(inputModel.UserName, inputModel.Email, inputModel.Password))
            {
                this.TempData["Error"] = RegisterAdminErrorMessage;
                return this.View(inputModel);
            }

            this.TempData["Success"] = string.Format(RegisterAdminSeccessMessage, inputModel.UserName);
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
        public async Task<IActionResult> RemoveAdmin(RemoveAdminInputModel inputModel)
        {
            try
            {
                string adminName = await this.usersService.RemoveAdmin(inputModel.Email);
                this.TempData["Success"] = string.Format(RemoveSuccessMessage, adminName);
            }
            catch
            {
                this.TempData["Error"] = string.Format(RemoveErrorMessage);
            }

            return this.RedirectToAction("Index", "Users");
        }
    }
}
