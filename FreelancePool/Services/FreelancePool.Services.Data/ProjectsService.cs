namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class ProjectsService : IProjectsService
    {
        private const int NumberOfRecentProjects = 6;
        private const int NumberOfPopularProject = 5;

        private readonly IDeletableEntityRepository<Project> projectsRepository;
        private readonly IRepository<CategoryProject> projectCategoryRepository;
        private readonly IRepository<ProjectOfferUser> projectOfferUserRepository;

        public ProjectsService(
            IDeletableEntityRepository<Project> projectsRepository,
            IRepository<CategoryProject> projectCategoryRepository,
            IRepository<ProjectOfferUser> projectOfferUserRepository)
        {
            this.projectsRepository = projectsRepository;
            this.projectCategoryRepository = projectCategoryRepository;
            this.projectOfferUserRepository = projectOfferUserRepository;
        }

        public async Task CloseAsync(int id, string executorId)
        {
            var project = this.projectsRepository.All().Where(p => p.Id == id).FirstOrDefault();

            if (project != null)
            {
                project.Status = ProjectStatus.Finished;
                project.ExecutorId = executorId;

                await this.projectsRepository.SaveChangesAsync();
            }
        }

        public async Task<int> CreateAsync(string title, string description, string authorId, ICollection<int> categoriesId, IEnumerable<string> usersIds)
        {
            var project = new Project
            {
                Title = title,
                Description = description,
                Status = ProjectStatus.Open,
                AuthorId = authorId,
            };

            await this.projectsRepository.AddAsync(project);
            await this.projectsRepository.SaveChangesAsync();

            foreach (var categoryId in categoriesId)
            {
                var projectCategory = new CategoryProject
                {
                    Project = project,
                    CategoryId = categoryId,
                };

                await this.projectCategoryRepository.AddAsync(projectCategory);
                await this.projectCategoryRepository.SaveChangesAsync();
            }

            if (usersIds.Count() != 0)
            {
                foreach (var userId in usersIds)
                {
                    var projectOfferUser = new ProjectOfferUser
                    {
                        Project = project,
                        UserId = userId,
                    };

                    await this.projectOfferUserRepository.AddAsync(projectOfferUser);
                    await this.projectOfferUserRepository.SaveChangesAsync();
                }
            }

            return project.Id;
        }

        public T GetById<T>(int id)
        {
            return this.projectsRepository.All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public IEnumerable<T> GetLastProjects<T>(ApplicationUser user)
        {
            var projects = this.projectsRepository.All().Where(p => (int)p.Status == 1);

            if (user != null && user.UserCategories.Count > 0)
            {
                List<string> userCategories = user.UserCategories.Select(uc => uc.Category.Name).ToList();

                projects = projects.Where(p => p.ProjectCategories.Select(pc => pc.Category.Name).Any(x => userCategories.Contains(x)));
            }

            return projects
                .OrderByDescending(p => p.CreatedOn)
                .Take(NumberOfRecentProjects)
                .To<T>()
                .ToList();
        }

        public string GetAuthorId(int id)
        {
            return this.projectsRepository.All()
                .Where(p => p.Id == id)
                .Select(p => p.AuthorId)
                .FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.projectsRepository.All()
                .Where(p => (int)p.Status == 1)
                .OrderByDescending(p => p.CreatedOn)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetMostWanted<T>()
        {
            var projects = this.projectsRepository.All()
                .Where(p => (int)p.Status == 1)
                .OrderByDescending(p => p.AppliedUsers.Count)
                .Take(NumberOfPopularProject);

            return projects.To<T>().ToList();
        }

        public string GetTitleById(int projectId)
        {
            return this.projectsRepository.All()
                .Where(p => p.Id == projectId)
                .Select(p => p.Title)
                .FirstOrDefault();
        }
    }
}
