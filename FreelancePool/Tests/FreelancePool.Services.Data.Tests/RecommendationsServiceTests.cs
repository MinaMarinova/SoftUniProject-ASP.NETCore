namespace FreelancePool.Services.Data.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;
    using FreelancePool.Data.Repositories;
    using FreelancePool.Services.Data.Tests.Common;
    using Xunit;

    public class RecommendationsServiceTests
    {
        [Fact]
        public async Task CreateAsyncAddsEntity()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            var recommendationsRepository = new EfDeletableEntityRepository<Recommendation>(dbContext);
            var service = new RecommendationsService(recommendationsRepository);

            await service.CreateAsync("TestUser1", "TestUser8", "Recommendation");

            Assert.Equal(1, recommendationsRepository.All().Count());
        }
    }
}
