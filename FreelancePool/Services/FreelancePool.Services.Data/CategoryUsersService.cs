namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;

    public class CategoryUsersService : ICategoryUsersService
    {
        private readonly IRepository<CategoryUser> categoryUsersRepository;

        public CategoryUsersService(IRepository<CategoryUser> categoryUsersRepository)
        {
            this.categoryUsersRepository = categoryUsersRepository;
        }

        public async Task CreateAsync(string userId, ICollection<int> categoriesId)
        {
            foreach (var categoryId in categoriesId)
            {
                var categoryUser = new CategoryUser
                {
                    CategoryId = categoryId,
                    UserId = userId,
                };

                await this.categoryUsersRepository.AddAsync(categoryUser);
                await this.categoryUsersRepository.SaveChangesAsync();
            }
        }
    }
}
