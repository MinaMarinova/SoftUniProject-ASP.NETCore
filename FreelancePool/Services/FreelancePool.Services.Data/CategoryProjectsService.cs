namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;

    public class CategoryProjectsService : ICategoryProjectsService
    {
        private readonly IRepository<CategoryProject> categoryProjectsRepository;

        public CategoryProjectsService(IRepository<CategoryProject> categoryProjectsRepository)
        {
            this.categoryProjectsRepository = categoryProjectsRepository;
        }

        public async Task CreateAsync(int projectId, ICollection<int> categoriesId)
        {
            foreach (var categoryId in categoriesId)
            {
                var projectCategory = new CategoryProject
                {
                    ProjectId = projectId,
                    CategoryId = categoryId,
                };

                await this.categoryProjectsRepository.AddAsync(projectCategory);
                await this.categoryProjectsRepository.SaveChangesAsync();
            }
        }
    }
}
