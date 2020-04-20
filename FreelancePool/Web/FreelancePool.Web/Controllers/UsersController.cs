namespace FreelancePool.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data.Models;
    using FreelancePool.Services;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.Helpers;
    using FreelancePool.Web.ViewModels.Categories;
    using FreelancePool.Web.ViewModels.Components;
    using FreelancePool.Web.ViewModels.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly ICloudinaryService cloudinaryService;

        private readonly IDataProtector protector;

        public UsersController(
            ICategoriesService categoriesService,
            UserManager<ApplicationUser> userManager,
            IUsersService usersService,
            ICloudinaryService cloudinaryService,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            this.categoriesService = categoriesService;
            this.userManager = userManager;
            this.usersService = usersService;
            this.cloudinaryService = cloudinaryService;
            this.protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        // TODO: To add API Key at appsettings.json for TinyMCE
        [Authorize]
        public IActionResult CreateProfile()
        {
            var userId = this.userManager.GetUserId(this.User);
            var modelView = this.usersService.GetUserById<CreateProfileInputModel>(userId);

            if (modelView == null)
            {
                return this.NotFound();
            }

            var allCategories = new AllCategoriesViewModel
            {
                Categories = this.categoriesService.GetAll<CategoryListViewModel>(),
            };

            modelView.AllCategories = allCategories;

            return this.View(modelView);
        }

        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> IsEmailUnique(string email)
        {
            var userId = this.userManager.GetUserId(this.User);
            var user = this.usersService.GetUserById<CreateProfileInputModel>(userId);
            var userEmail = user.Email;

            if (userEmail == email)
            {
                return this.Json(true);
            }

            var filterUser = await this.userManager.FindByEmailAsync(email);

            if (filterUser != null)
            {
                return this.Json($"Email {email} is already in use!");
            }
            else
            {
                return this.Json(true);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProfile(CreateProfileInputModel inputModel)
        {
            var userId = this.userManager.GetUserId(this.User);

            // TODO: To edit error messagies in the ViewModel;
            if (!this.ModelState.IsValid)
            {
                inputModel.PhotoUrl = this.usersService.GetUserById<CreateProfileInputModel>(userId).PhotoUrl;

                var allCategories = new AllCategoriesViewModel
                {
                    Categories = this.categoriesService.GetAll<CategoryListViewModel>(),
                };

                inputModel.AllCategories = allCategories;

                return this.View(inputModel);
            }

            if (inputModel.NewPhoto != null)
            {
                inputModel.PhotoUrl = await this.cloudinaryService.UploadPhotoAsync(inputModel.NewPhoto, "User", "Users");
            }

            await this.usersService.CreateAProfileAsync(userId, inputModel.UserName, inputModel.PhotoUrl, inputModel.Email, inputModel.Summary, inputModel.PhoneNumber, inputModel.CategoriesId);

            return this.RedirectToAction(nameof(this.Profile), new { id = userId });
        }

        public IActionResult Profile(string id)
        {
            string decryptedId = id;

            if (id != this.userManager.GetUserId(this.User))
            {
                decryptedId = this.protector.Unprotect(id);
            }

            var freelancerViewModel = this.usersService.GetUserById<DetailsViewModel>(decryptedId);

            if (freelancerViewModel == null)
            {
                return this.NotFound();
            }

            foreach (var recommendation in freelancerViewModel.Recommendations)
            {
                recommendation.Author.EncryptedId = this.protector.Protect(recommendation.Author.Id);
            }

            return this.View(freelancerViewModel);
        }

        [HttpGet]
        [HttpPost]
        public IActionResult All(AllFreelancersViewModel viewModel)
        {
            viewModel.Freelancers = this.usersService.GetAll<FreelancerViewModel>();

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
                f.Categories = f.Categories.Take(3);
                f.EncryptedId = this.protector.Protect(f.Id);
                return f;
            }).ToList();

            viewModel.TopFreelancers = this.usersService.GetTop<FreelancerViewModel>()
                .Select(f =>
            {
                f.Categories = f.Categories.Take(3);
                f.EncryptedId = this.protector.Protect(f.Id);
                return f;
            }).ToList();

            viewModel.RecentlyJoined = this.usersService.GetRecent<FreelancerViewModel>().Select(f =>
            {
                f.Categories = f.Categories.Take(3);
                f.EncryptedId = this.protector.Protect(f.Id);
                return f;
            }).ToList();

            return this.View(viewModel);
        }
    }
}
