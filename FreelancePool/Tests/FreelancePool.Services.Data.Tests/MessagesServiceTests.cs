namespace FreelancePool.Services.Data.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data;
    using FreelancePool.Data.Models;
    using FreelancePool.Data.Repositories;
    using FreelancePool.Services.Data.Tests.Common;
    using Xunit;

    public class MessagesServiceTests
    {
        [Fact]
        public async Task CreateAddsEntityToTheDatabase()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Message>(dbContext);
            var service = new MessagesService(repository);

            await service.Create("test", 1, "TestUser1");

            Assert.Equal(2, repository.All().Count());
        }

        [Fact]
        public async Task DeleteRemovesTheEntity()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Message>(dbContext);
            var service = new MessagesService(repository);

            await service.Delete(1);

            Assert.Equal(0, repository.All().Count());
        }

        [Fact]
        public async Task DeleteReturnsCorectProjectId()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Message>(dbContext);
            var service = new MessagesService(repository);

            Assert.Equal(2, await service.Delete(1));
        }

        private async Task SeedDataAsync(ApplicationDbContext dbContext)
        {
            dbContext.Messages.Add(new Message
            {
                Id = 1,
                Content = "TestMessage",
                ProjectId = 2,
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
