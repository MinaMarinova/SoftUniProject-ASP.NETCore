namespace FreelancePool.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data;
    using FreelancePool.Data.Models;
    using FreelancePool.Data.Repositories;
    using FreelancePool.Services.Data.Tests.Common;
    using FreelancePool.Web.ViewModels.Categories;

    using Xunit;

    public class CategoriesServiceTests
    {
        [Fact]
        public async Task GetCategoryIdByNameReturnsCorrectValue()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            Assert.Equal(2, service.GetCategoryIdByName("Art"));
        }

        [Fact]
        public async Task GetCategoryIdByNameReturnsZeroIfNameIsInvalid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            Assert.Equal(0, service.GetCategoryIdByName("Arts"));
        }

        [Fact]
        public async Task GetAllReturnsAllEntities()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            var count = 0;
            IEnumerable<CategoryListViewModel> allCategories = service.GetAll<CategoryListViewModel>();

            foreach (var category in allCategories)
            {
                count++;
            }

            Assert.Equal(4, count);
        }

        [Fact]
        public async Task AddShouldIncreaseTheNumberOfEntities()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);
            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            var expectedCount = repository.All().Count() + 1;
            await service.Add("Music", "someUrl");

            Assert.Equal(expectedCount, repository.All().Count());
        }

        [Fact]
        public async Task AddShouldAddTheNameOfTheCategory()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);
            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            await service.Add("Music", "someUrl");
            var categoriesNames = repository.All().Select(c => c.Name).ToList();

            Assert.Contains("Music", categoriesNames);
        }

        [Fact]
        public async Task AddReturnsOneIfEntityIsAdded()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);
            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            Assert.Equal(1, await service.Add("Music", "someUrl"));
        }

        [Fact]
        public async Task GetNameByIdReturnsCorrectValue()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            Assert.Equal("Writing", service.GetNameById(1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        [InlineData(4)]
        public async Task GetNameByIdReturnsNullIfIdIsInvalid(int id)
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            Assert.Null(service.GetNameById(id));
        }

        [Fact]
        public async Task DeleteShouldDecreaseTheTableWithOneEntity()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            var count = repository.All().Count() - 1;
            await service.Delete("Writing");

            Assert.Equal(count, repository.All().Count());

        }

        [Fact]
        public async Task DeleteShouldDeleteRightEntity()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            await service.Delete("Writing");

            var categoriesNames = repository.All().Select(c => c.Name);

            Assert.DoesNotContain("Writing", categoriesNames);
        }

        [Fact]
        public async Task DeleteThrowsArgumentNullExceptionIfNameIsNotValid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.Delete("Writings"));
        }

        [Fact]
        public async Task DeleteReturnsOneIfEntityIsDeleted()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);
            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            Assert.Equal(1, await service.Delete("Writing"));
        }

        [Fact]
        public async Task EditShouldChangeCategoryName()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);
            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            string expectedResult = "Edited";

            await service.Edit("Writing", expectedResult);

            string actualResult = repository.All()
                .Where(c => c.Id == 1)
                .Select(c => c.Name)
                .FirstOrDefault();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task EditShouldReturnOne()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);
            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            var actualResult = await service.Edit("Writing", "Edit");

            Assert.Equal(1, actualResult);
        }

        [Fact]
        public async Task EditThrowsArgumentNullExceptionIfNameIsNotValid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataAsync(dbContext);

            var repository = new EfDeletableEntityRepository<Category>(dbContext);
            var service = new CategoriesService(repository);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.Edit("Wrong", "Edit"));
        }


        private async Task SeedDataAsync(ApplicationDbContext dbContext)
        {
            dbContext.Categories.Add(new Category
            {
                Id = 1,
                Name = "Writing",
            });

            dbContext.Categories.Add(new Category
            {
                Id = 2,
                Name = "Art",
            });

            dbContext.Categories.Add(new Category
            {
                Id = 5,
                Name = "Consulting",
            });

            dbContext.Categories.Add(new Category
            {
                Id = 8,
                Name = "Sales & Marketing",
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
