﻿namespace FreelancePool.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.Helpers;
    using FreelancePool.Web.ViewModels.Categories;
    using FreelancePool.Web.ViewModels.Components;
    using FreelancePool.Web.ViewModels.Projects;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ProjectsController : BaseController
    {
        private const string ActionDeniedMessage = "You are not allowed to close this project!";
        private const string NotValidEmailErrorMessage = "There is no freelancer with {0} email!";
        private const string ApplySuccessMessage = "You have successfully applied for project: {0}";
        private const string PostSuccessMessage = "You have successfully posted a project: {0}";

        private readonly IProjectsService projectsService;
        private readonly ICategoriesService categoriesService;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRecommendationsService recommendationsService;
        private readonly IUserCandidateProjectsService userCandidateProjectsServices;
        private readonly ICategoryProjectsService categoryProjectsServices;
        private readonly IProjectOfferUsersService projectOfferUsersServices;
        private readonly IDataProtector protector;

        public ProjectsController(
            IProjectsService projectsService,
            ICategoriesService categoriesService,
            IUsersService usersService,
            UserManager<ApplicationUser> userManager,
            IRecommendationsService recommendationsService,
            IUserCandidateProjectsService userCandidateProjectsServices,
            ICategoryProjectsService categoryProjectsServices,
            IProjectOfferUsersService projectOfferUsersServices,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            this.projectsService = projectsService;
            this.categoriesService = categoriesService;
            this.usersService = usersService;
            this.userManager = userManager;
            this.recommendationsService = recommendationsService;
            this.userCandidateProjectsServices = userCandidateProjectsServices;
            this.categoryProjectsServices = categoryProjectsServices;
            this.projectOfferUsersServices = projectOfferUsersServices;
            this.protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }

        [Authorize]
        public IActionResult Post()
        {
            var allCategories = new AllCategoriesViewModel
            {
                Categories = this.categoriesService.GetAll<CategoryListViewModel>(),
            };

            var modelView = new ProjectPostInputViewModel
            {
                AllCategories = allCategories,
            };

            return this.View(modelView);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(ProjectPostInputViewModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var allCategories = new AllCategoriesViewModel
                {
                    Categories = this.categoriesService.GetAll<CategoryListViewModel>(),
                };

                inputModel.AllCategories = allCategories;
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var usersIds = this.usersService.GetUsersIdsFromEmailsString(inputModel.UsersEmails);

            var projectId = await this.projectsService.CreateAsync(inputModel.Title, inputModel.Description, user.Id);

            await this.categoryProjectsServices.CreateAsync(projectId, inputModel.CategoriesId);

            if (usersIds.Count != 0)
            {
                await this.projectOfferUsersServices.CreateAsync(projectId, usersIds);
            }

            this.TempData["SuccessPost"] = string.Format(PostSuccessMessage, inputModel.Title);

            return this.RedirectToAction("Profile", "Users", new { id = user.Id });
        }

        public IActionResult DetailsById(int id)
        {
            var projectViewModel = this.projectsService.GetById<ProjectDetailsViewModel>(id);

            if (projectViewModel == null)
            {
                return this.NotFound();
            }

            projectViewModel.AuthorEncryptedId = this.protector.Protect(projectViewModel.AuthorId);

            foreach (var message in projectViewModel.MessagesLeft)
            {
                message.Author.EncryptedId = this.protector.Protect(message.Author.Id);
            }

            foreach (var offeredUser in projectViewModel.Suggested)
            {
                offeredUser.EncryptedId = this.protector.Protect(offeredUser.Id);
            }

            foreach (var appliedUser in projectViewModel.Candidates)
            {
                appliedUser.EncryptedId = this.protector.Protect(appliedUser.Id);
            }

            return this.View(projectViewModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.FreelancerRoleName)]
        public async Task<IActionResult> DetailsById(ProjectDetailsViewModel viewModel)
        {
            var projectId = viewModel.Id;

            if (projectId == 0)
            {
                return this.NotFound();
            }

            var userId = this.userManager.GetUserId(this.User);

            await this.userCandidateProjectsServices.ApplyAsync(userId, projectId);
            var projectTitle = this.projectsService.GetTitleById(projectId);

            this.TempData["SuccessApplied"] = string.Format(ApplySuccessMessage, projectTitle);

            return this.RedirectToAction("Profile", "Users", new { id = userId });
        }

        [Authorize]
        public IActionResult Close(int id)
        {
            var authorId = this.projectsService.GetAuthorId(id);

            if (authorId != this.userManager.GetUserId(this.User))
            {
                this.TempData["Error"] = ActionDeniedMessage;
                return this.RedirectToAction(nameof(this.DetailsById), new { id });
            }

            var projectViewModel = this.projectsService.GetById<CloseProjectViewModel>(id);

            if (projectViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(projectViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult IsEmailValid()
        {
            var email = this.HttpContext.Request.QueryString.Value.Split("=")[1].Replace("%40", "@");

            var filterId = this.usersService.GetUserIdByEmail(email);

            if (filterId == null)
            {
                return this.Json(string.Format(NotValidEmailErrorMessage, email));
            }

            return this.Json(true);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Close(CloseProjectViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var authorId = this.projectsService.GetAuthorId(viewModel.Id);

            if (authorId != this.userManager.GetUserId(this.User))
            {
                this.TempData["Error"] = ActionDeniedMessage;
                this.RedirectToAction(nameof(this.DetailsById), new { id = viewModel.Id });
            }

            var executorId = this.usersService.GetUserIdByEmail(viewModel.ExecutorEmail);

            this.TempData["Error"] = null;

            await this.usersService.RateFreelancerAsync(authorId, executorId, viewModel.StarGivenOrTaken, viewModel.Recommendation);

            await this.recommendationsService.CreateAsync(authorId, executorId, viewModel.Recommendation);

            await this.projectsService.CloseAsync(viewModel.Id, executorId);

            return this.RedirectToAction(nameof(this.DetailsById), new { id = viewModel.Id });
        }

        [HttpGet]
        [HttpPost]
        public IActionResult All(AllProjectsViewModel viewModel)
        {
            var categories = this.categoriesService.GetAll<CategoryListViewModel>();

            var allCategories = new AllCategoriesViewModel
            {
                Categories = categories,
            };

            viewModel.AllCategories = allCategories;
            viewModel.AllProjects = this.projectsService.GetAll<ProjectViewModel>();

            if (viewModel.SelectedCategoriesId.Count() > 0)
            {
                viewModel.AllProjects = viewModel.AllProjects.Where(p => p.CategoriesId.Any(c => viewModel.SelectedCategoriesId.Contains(c)));
            }

            viewModel.MostWanted = this.projectsService.GetMostWanted<ProjectTitleAndApplicantsViewModel>();

            return this.View(viewModel);
        }
    }
}
