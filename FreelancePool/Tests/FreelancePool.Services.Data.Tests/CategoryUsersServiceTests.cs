namespace FreelancePool.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;
    using FreelancePool.Data.Repositories;
    using FreelancePool.Services.Data.Tests.Common;
    using Xunit;

    public class CategoryUsersServiceTests
    {
        [Fact]
        public async Task CreateAsyncAddsEntity()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            var categoryUsersRepository = new EfRepository<CategoryUser>(dbContext);
            var service = new CategoryUsersService(categoryUsersRepository);

            var categoriesIds = new List<int> { 1, 3 };

            await service.CreateAsync("TestUser1", categoriesIds);

            Assert.Equal(2, categoryUsersRepository.All().Count());
        }
    }
}
