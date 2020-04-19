namespace FreelancePool.Web.Controllers
{
    using System.Linq;

    using FreelancePool.Common;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.Helpers;
    using FreelancePool.Web.ViewModels.Categories;
    using FreelancePool.Web.ViewModels.Components;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IProjectsService projectsService;
        private readonly IUsersService usersService;

        private readonly IDataProtector protector;

        public CategoriesController(
            ICategoriesService categoriesService,
            IProjectsService projectsService,
            IUsersService usersService,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            this.categoriesService = categoriesService;
            this.projectsService = projectsService;
            this.usersService = usersService;
            this.protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        public IActionResult DetailsById(CategoryDetailsViewModel viewModel)
        {
            viewModel.Name = this.categoriesService.GetNameById(viewModel.Id);

            if (viewModel.Name == null)
            {
                return this.NotFound();
            }

            viewModel.Freelancers = this.usersService.GetAll<FreelancerViewModel>()
                .Where(f => f.Categories.Any(c => c == viewModel.Name));

            if (viewModel.Order == GlobalConstants.AlphabeticalOrder)
            {
                viewModel.Freelancers = viewModel.Freelancers.OrderBy(f => f.UserName).ToList();
            }

            if (viewModel.Order == GlobalConstants.RatingOrder)
            {
                viewModel.Freelancers = viewModel.Freelancers.OrderByDescending(f => f.Stars).ToList();
            }

            viewModel.Freelancers.Select(f =>
            {
                f.EncryptedId = this.protector.Protect(f.Id);
                return f;
            }).ToList();

            viewModel.Projects = this.projectsService.GetAll<ProjectViewModel>()
                .Where(p => p.CategoriesId.Any(id => id == viewModel.Id));

            return this.View(viewModel);
        }
    }
}
