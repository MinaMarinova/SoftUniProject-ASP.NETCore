namespace FreelancePool.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FreelancePool.Common;
    using FreelancePool.Data;
    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Data.Repositories;
    using FreelancePool.Services.Data.Tests.Common;
    using FreelancePool.Web.ViewModels.Components;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using Xunit;

    public class UsersServiceTests
    {
        [Fact]
        public async Task GetRandomFreelancersReturnsOnlyAssignedtoCategoryUsers()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var user = usersRepository.All().LastOrDefault();
            var freelancers = service.GetRandomFreelancers<FreelancerViewModel>(user);

            Assert.All(freelancers, freelancer => Assert.NotEmpty(freelancer.Categories));
        }

        [Fact]
        public async Task GetRandomFreelancersReturnsFreelancersFromCertainCategory()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var user = usersRepository.All().Where(u => u.UserCategories.Count == 1 && u.UserCategories.All(uc => uc.CategoryId == 5)).FirstOrDefault();

            var freelancers = service.GetRandomFreelancers<FreelancerViewModel>(user);

            Assert.All(freelancers, freelancer => Assert.Contains("Consulting", freelancer.Categories));
        }

        private UsersService InitializeService(EfDeletableEntityRepository<ApplicationUser> usersRepository, ApplicationDbContext dbContext)
        {
            var userCandidateProjectsRepository = new EfRepository<UserCandidateProject>(dbContext);
            var categoryUsersRepository = new EfRepository<CategoryUser>(dbContext);
            var recommendationsRepository = new EfDeletableEntityRepository<Recommendation>(dbContext);
            var userManager = this.GetUserManagerMock(dbContext);

            var service = new UsersService(usersRepository, userCandidateProjectsRepository, categoryUsersRepository, recommendationsRepository, userManager.Object);

            return service;
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock(ApplicationDbContext context)
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), GlobalConstants.FreelancerRoleName))
                .ReturnsAsync(IdentityResult.Success);

            return userManagerMock;
        }

        private static async Task SeedDataAsync(ApplicationDbContext dbContext)
        {
            // Seeding some categories
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

            // Seeding Users
            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser1",
                UserName = "TestUser1",
            });

            await AddUserToCategoriesAsync(new int[] { 1, 8 }, "TestUser1", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser2",
                UserName = "TestUser2",
            });

            await AddUserToCategoriesAsync(new int[] { 1, 2 }, "TestUser2", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser3",
                UserName = "TestUser3",
            });

            await AddUserToCategoriesAsync(new int[] { 5, 8 }, "TestUser3", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser4",
                UserName = "TestUser4",
            });

            await AddUserToCategoriesAsync(new int[] { 5 }, "TestUser4", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser5",
                UserName = "TestUser5",
            });

            await AddUserToCategoriesAsync(new int[] { 2, 8 }, "TestUser5", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser6",
                UserName = "TestUser6",
            });

            await AddUserToCategoriesAsync(new int[] { 1, 5 }, "TestUser6", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser7",
                UserName = "TestUser7",
            });

            await AddUserToCategoriesAsync(new int[] { 8 }, "TestUser7", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser8",
                UserName = "TestUser8",
            });

            await AddUserToCategoriesAsync(new int[] { 2, 5 }, "TestUser8", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser9",
                UserName = "TestUser9",
            });

            await AddUserToCategoriesAsync(new int[] { 2 }, "TestUser9", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser10",
                UserName = "TestUser10",
            });
            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser11",
                UserName = "TestUser11",
            });
            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser12",
                UserName = "TestUser12",
            });

            await dbContext.SaveChangesAsync();
        }

        private static async Task AddUserToCategoriesAsync(int[] categoriesId, string userId, ApplicationDbContext dbContext)
        {
            foreach (var categoryId in categoriesId)
            {
                var categoryUser = new CategoryUser
                {
                    UserId = userId,
                    CategoryId = categoryId,
                };

                dbContext.CategoriesUsers.Add(categoryUser);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
