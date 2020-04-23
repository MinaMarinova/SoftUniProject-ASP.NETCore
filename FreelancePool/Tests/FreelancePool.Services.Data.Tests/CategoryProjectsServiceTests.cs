namespace FreelancePool.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;
    using FreelancePool.Data.Repositories;
    using FreelancePool.Services.Data.Tests.Common;
    using Xunit;

    public class CategoryProjectsServiceTests
    {
        [Fact]
        public async Task CreateAsyncAddsEntity()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryProjectsRepository = new EfRepository<CategoryProject>(dbContext);
            var service = new CategoryProjectsService(categoryProjectsRepository);

            var categoriesIds = new List<int> { 4, 6 };

            await service.CreateAsync(1, categoriesIds);

            Assert.Equal(2, categoryProjectsRepository.All().Count());
        }
    }
}
