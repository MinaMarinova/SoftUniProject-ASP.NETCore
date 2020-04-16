namespace FreelancePool.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using Microsoft.Extensions.DependencyInjection;

    public class ProjectsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var categoriesService = serviceProvider.GetService<ICategoriesService>();
            var categoryProjectsRepository = serviceProvider.GetService<IRepository<CategoryProject>>();

            if (dbContext.Projects.Any())
            {
                return;
            }

            var projectEdit = new Project
            {
                Title = "Edit 2000 word personal story",
                Description = "The 2000 word personal story written by a high school student needs grammar correction and suggestions to make it more interesting. This is an easy project for anyone who loves writing. To bit, tell me about your expertise and when you can start and finish this project",
                AuthorId = "4edc3011-7362-4b46-ab3a-9d0ad1188f98",
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectEdit);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectEdit, GlobalConstants.WritingCategoryName, categoriesService, categoryProjectsRepository);

            var projectCovid = new Project
            {
                Title = "COVID-19 articles writer",
                Description = "I need somebody to write articles for COVID-19. The engagement is for at least 5 articles per day. They can be news from the day, interesting facts or even conducted interviews with experts.",
                AuthorId = "59d194d3-6207-463b-ac05-2639687c9039",
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectCovid);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectCovid, GlobalConstants.WritingCategoryName, categoriesService, categoryProjectsRepository);

            var projectLogo = new Project
            {
                Title = "Design a logo for e-shop (urgent)",
                Description = "I’m looking for a very good designer who can make a logo for our fashion e-shop within 48 hours. Portfolio needed.",
                AuthorId = "59d194d3-6207-463b-ac05-2639687c9039",
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectLogo);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectLogo, GlobalConstants.ArtCategoryName, categoriesService, categoryProjectsRepository);

            var projectPhoto = new Project
            {
                Title = "Photographer for catalog",
                Description = "I’m preparing the first catalog of my most important inventions, so I need a professional photographer. There would be 10 sections with 5 photos each.",
                AuthorId = "ea1f14b4-8e1d-40d9-89a3-662644ed7a32",
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectPhoto);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectPhoto, GlobalConstants.ArtCategoryName, categoriesService, categoryProjectsRepository);

            var projectBlog = new Project
            {
                Title = "Content writer for a blog",
                Description = "I have a blog website where I post songs, videos, entertainment e.t.c. So I need a very good content to get my site approved by Google AdSense.",
                AuthorId = "851014c0-33e7-4fd2-89bf-e38200167b99",
                ExecutorId = "c501c879-6027-452c-ad0d-5684ab8cb0cb",
                Status = ProjectStatus.Finished,
            };

            await dbContext.Projects.AddAsync(projectBlog);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectBlog, GlobalConstants.WritingCategoryName, categoriesService, categoryProjectsRepository);

            var projectWeb = new Project
            {
                Title = "Build me a website",
                Description = "Hi, I need one of my websites updated and another one created. The first one is for hotel booking and the other one for travelling.",
                AuthorId = "59d194d3-6207-463b-ac05-2639687c9039",
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectWeb);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectWeb, GlobalConstants.SoftwareDevCategoryName, categoriesService, categoryProjectsRepository);

            var projectSite = new Project
            {
                Title = "Developer for stand-alone website",
                Description = "We are looking for developer for small, interactive stand-alone website. The website behaves similarly to a decision tree in which the user chooses options in specific orders that leads them to a conclusion. Upon reaching the conclusion, the user will be given an option to receive an email of the conclusion or start personal data entry to receive more information",
                AuthorId = "851014c0-33e7-4fd2-89bf-e38200167b99",
                Status = ProjectStatus.Finished,
                ExecutorId = "a3ef8be3-9e2d-4791-9676-9719718b8a0b",
            };

            await dbContext.Projects.AddAsync(projectSite);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectSite, GlobalConstants.SoftwareDevCategoryName, categoriesService, categoryProjectsRepository);

            var projectCare = new Project
            {
                Title = "Child care for weekends",
                Description = "We are looking for a kind woman to look after our 10-years old boy for two hours in the weekends. Patience and tendency to teach are most appreciated.",
                AuthorId = "4edc3011-7362-4b46-ab3a-9d0ad1188f98",
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectCare);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectCare, GlobalConstants.SocialCareCategoryName, categoriesService, categoryProjectsRepository);
        }

        private static async Task AddProjectToCategoryAsync(Project project, string categoryName, ICategoriesService categoriesService, IRepository<CategoryProject> categoryProjectsRepository)
        {
            var categoryId = categoriesService.GetCategoryIdByName(categoryName);

            var categoryProject = new CategoryProject
            {
                CategoryId = categoryId,
                ProjectId = project.Id,
            };

            await categoryProjectsRepository.AddAsync(categoryProject);
        }
    }
}
