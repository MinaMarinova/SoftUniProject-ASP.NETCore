namespace FreelancePool.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using FreelancePool.Services;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Administration.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private const string CreateSuccessMessage = "You have successfully created {0} category!";
        private const string CreateErrorMessage = "The creation failed!";
        private const string CategoryNotFoundMessage = "There is no {0} category in the database!";
        private const string DeleteSuccessMessage = "You have successfully deleted {0} category!";
        private const string DeleteErrorMessage = "Failed to delete {0} cateogory!";
        private const string EditErrorMessage = "Failed to edit {0} category!";
        private const string EditSuccessMessage = "The new category's name is {0}!";

        private readonly ICategoriesService categoriesService;
        private readonly ICloudinaryService cloudinaryService;

        public CategoriesController(ICategoriesService categoriesService, ICloudinaryService cloudinaryService)
        {
            this.categoriesService = categoriesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            string iconUrl = await this.cloudinaryService.UploadPhotoAsync(inputModel.Icon, "User", "Users");

            if (await this.categoriesService.Add(inputModel.Name, iconUrl) > 0)
            {
                this.TempData["Success"] = string.Format(CreateSuccessMessage, inputModel.Name);
            }
            else
            {
                this.TempData["Error"] = string.Format(CreateErrorMessage);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(ChooseCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                if (await this.categoriesService.Delete(inputModel.Name) > 0)
                {
                    this.TempData["Success"] = string.Format(DeleteSuccessMessage, inputModel.Name);
                }
                else
                {
                    this.TempData["Error"] = string.Format(DeleteErrorMessage, inputModel.Name);
                }
            }
            catch (ArgumentNullException)
            {
                this.TempData["Error"] = string.Format(CategoryNotFoundMessage, inputModel.Name);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Edit(EditCategoryViewModel viewModel)
        {
            var categoryId = this.categoriesService.GetCategoryIdByName(viewModel.Name);
            if (categoryId == 0)
            {
                this.TempData["Error"] = string.Format(CategoryNotFoundMessage, viewModel.Name);
                return this.RedirectToAction(nameof(this.Index));
            }

            viewModel.CurrentName = viewModel.Name;
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAction(EditCategoryViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Edit));
            }

            try
            {
                if (await this.categoriesService.Edit(viewModel.CurrentName, viewModel.Name) > 0)
                {
                    this.TempData["Success"] = string.Format(EditSuccessMessage, viewModel.Name);
                }
                else
                {
                    this.TempData["Error"] = string.Format(EditErrorMessage, viewModel.Name);
                }
            }
            catch (ArgumentNullException)
            {
                this.TempData["Error"] = string.Format(CategoryNotFoundMessage, viewModel.Name);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
