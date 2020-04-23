namespace FreelancePool.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Data;
    using FreelancePool.Web.ViewModels.Components;
    using Microsoft.Extensions.DependencyInjection;

    public class ProjectsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var categoriesService = serviceProvider.GetService<ICategoriesService>();
            var categoryProjectsRepository = serviceProvider.GetService<IRepository<CategoryProject>>();
            var usersService = serviceProvider.GetService<IUsersService>();

            if (dbContext.Projects.Any())
            {
                return;
            }

            var authorpPojectEdit = usersService.GetUserByName<FreelancerViewModel>("Ivan Popov");
            var projectEdit = new Project
            {
                Title = "Edit 2000 word personal story",
                Description = "The 2000 word personal story written by a high school student needs grammar correction and suggestions to make it more interesting. This is an easy project for anyone who loves writing. To bit, tell me about your expertise and when you can start and finish this project",
                AuthorId = authorpPojectEdit.Id,
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectEdit);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectEdit, GlobalConstants.WritingCategoryName, categoriesService, categoryProjectsRepository);

            var authorProjectCovid = usersService.GetUserByName<FreelancerViewModel>("Milena Popova");
            var projectCovid = new Project
            {
                Title = "COVID-19 articles writer",
                Description = "I need somebody to write articles for COVID-19. The engagement is for at least 5 articles per day. They can be news from the day, interesting facts or even conducted interviews with experts.",
                AuthorId = authorProjectCovid.Id,
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectCovid);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectCovid, GlobalConstants.WritingCategoryName, categoriesService, categoryProjectsRepository);

            var authorProjectLogo = usersService.GetUserByName<FreelancerViewModel>("Milena Popova");
            var projectLogo = new Project
            {
                Title = "Design a logo for e-shop (urgent)",
                Description = "I’m looking for a very good designer who can make a logo for our fashion e-shop within 48 hours. Portfolio needed.",
                AuthorId = authorProjectLogo.Id,
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectLogo);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectLogo, GlobalConstants.ArtCategoryName, categoriesService, categoryProjectsRepository);

            var authorProjectPhoto = usersService.GetUserByName<FreelancerViewModel>("Leonardo Da Vinci");
            var projectPhoto = new Project
            {
                Title = "Photographer for catalog",
                Description = "I’m preparing the first catalog of my most important inventions, so I need a professional photographer. There would be 10 sections with 5 photos each.",
                AuthorId = authorProjectPhoto.Id,
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectPhoto);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectPhoto, GlobalConstants.ArtCategoryName, categoriesService, categoryProjectsRepository);

            var authorProjectBlog = usersService.GetUserByName<FreelancerViewModel>("Radio Yerevan");
            var executorProjectBlog = usersService.GetUserByName<FreelancerViewModel>("William Shakespeare");
            var projectBlog = new Project
            {
                Title = "Content writer for a blog",
                Description = "I have a blog website where I post songs, videos, entertainment e.t.c. So I need a very good content to get my site approved by Google AdSense.",
                AuthorId = authorProjectBlog.Id,
                ExecutorId = executorProjectBlog.Id,
                Status = ProjectStatus.Finished,
            };

            await dbContext.Projects.AddAsync(projectBlog);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectBlog, GlobalConstants.WritingCategoryName, categoriesService, categoryProjectsRepository);

            var authorProjectWeb = usersService.GetUserByName<FreelancerViewModel>("Milena Popova");
            var projectWeb = new Project
            {
                Title = "Build me a website",
                Description = "Hi, I need one of my websites updated and another one created. The first one is for hotel booking and the other one for travelling.",
                AuthorId = authorProjectWeb.Id,
                Status = ProjectStatus.Open,
            };

            await dbContext.Projects.AddAsync(projectWeb);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectWeb, GlobalConstants.SoftwareDevCategoryName, categoriesService, categoryProjectsRepository);

            var authorProjectSite = usersService.GetUserByName<FreelancerViewModel>("Uncle Sam");
            var executorProjectSite = usersService.GetUserByName<FreelancerViewModel>("Alan Turing");
            var projectSite = new Project
            {
                Title = "Developer for stand-alone website",
                Description = "We are looking for developer for small, interactive stand-alone website. The website behaves similarly to a decision tree in which the user chooses options in specific orders that leads them to a conclusion. Upon reaching the conclusion, the user will be given an option to receive an email of the conclusion or start personal data entry to receive more information",
                AuthorId = authorProjectSite.Id,
                Status = ProjectStatus.Finished,
                ExecutorId = executorProjectSite.Id,
            };

            await dbContext.Projects.AddAsync(projectSite);
            await dbContext.SaveChangesAsync();

            await AddProjectToCategoryAsync(projectSite, GlobalConstants.SoftwareDevCategoryName, categoriesService, categoryProjectsRepository);

            var authorProjectCare = usersService.GetUserByName<FreelancerViewModel>("Ivan Popov");
            var projectCare = new Project
            {
                Title = "Child care for weekends",
                Description = "We are looking for a kind woman to look after our 10-years old boy for two hours in the weekends. Patience and tendency to teach are most appreciated.",
                AuthorId = authorProjectCare.Id,
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
