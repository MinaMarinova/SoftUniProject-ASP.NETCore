namespace FreelancePool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public int GetCategoryIdByName(string categoryName)
        {
            return this.categoriesRepository.All()
                .Where(c => c.Name == categoryName).Select(c => c.Id).FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this.categoriesRepository.All().To<T>().ToList();
        }

        public string GetNameById(int id)
        {
            return this.categoriesRepository.All()
                .Where(c => c.Id == id)
                .Select(c => c.Name)
                .FirstOrDefault();
        }

        public async Task<int> Add(string name, string iconUrl)
        {
            var category = new Category
            {
                Name = name,
                IconURL = iconUrl,
            };

            await this.categoriesRepository.AddAsync(category);
            return await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task<int> Delete(string name)
        {
            var category = this.categoriesRepository.All()
                .Where(c => c.Name == name)
                .FirstOrDefault();

            if (category == null)
            {
                throw new ArgumentNullException();
            }

            category.IsDeleted = true;
            return await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task<int> Edit(string currentName, string name)
        {
            var category = this.categoriesRepository.All()
                .Where(c => c.Name == currentName)
                .FirstOrDefault();

            if (category == null)
            {
                throw new ArgumentNullException();
            }

            category.Name = name;
            return await this.categoriesRepository.SaveChangesAsync();
        }
    }
}
