﻿namespace FreelancePool.Web.ViewComponents
{
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.Helpers;
    using FreelancePool.Web.ViewModels.Components;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class FreelancersViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        private readonly IDataProtector protector;

        public FreelancersViewComponent(
            UserManager<ApplicationUser> userManager,
            IUsersService usersService,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            this.userManager = userManager;
            this.usersService = usersService;
            this.userRepository = userRepository;
            this.protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        // Inject IHttpContextAccessor for mocking
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await this.userRepository
               .GetByIdWithDeletedAsync(this.userManager.GetUserId(this.ViewContext.HttpContext.User));

            var freelancers = this.usersService
               .GetRandomFreelancers<FreelancerViewModel>(user).Select(f =>
               {
                   f.Categories = f.Categories.Take(3);
                   f.EncryptedId = this.protector.Protect(f.Id);
                   return f;
               }).ToList();

            return this.View(freelancers);
        }
    }
}
