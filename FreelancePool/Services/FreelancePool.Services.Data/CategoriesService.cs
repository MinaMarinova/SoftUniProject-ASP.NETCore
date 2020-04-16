namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

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
    }
}
