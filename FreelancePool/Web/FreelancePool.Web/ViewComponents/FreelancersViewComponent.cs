namespace FreelancePool.Web.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Components;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FreelancersViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public FreelancersViewComponent(
            UserManager<ApplicationUser> userManager,
            IUsersService usersService,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.userRepository = userRepository;
        }

        // Inject IHttpContextAccessor for mocking
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userRepository
               .GetByIdWithDeletedAsync(this.userManager.GetUserId(this.ViewContext.HttpContext.User));

            var freelancers = this.usersService
               .GetRandomEightUsers<FreelancerViewModel>(user);

            return this.View(freelancers);
        }
    }
}
