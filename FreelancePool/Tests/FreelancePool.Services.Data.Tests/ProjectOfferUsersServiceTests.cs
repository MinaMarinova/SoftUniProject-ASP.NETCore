namespace FreelancePool.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;
    using FreelancePool.Data.Repositories;
    using FreelancePool.Services.Data.Tests.Common;
    using Xunit;

    public class ProjectOfferUsersServiceTests
    {
        [Fact]
        public async Task CreateAsyncAddsEntity()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            var projectOfferUsersRepository = new EfRepository<ProjectOfferUser>(dbContext);
            var service = new ProjectOfferUsersService(projectOfferUsersRepository);

            var freelancers = new List<string> { "Test1", "Test2" };

            await service.CreateAsync(1, freelancers);

            Assert.Equal(2, projectOfferUsersRepository.All().Count());
        }
    }
}
