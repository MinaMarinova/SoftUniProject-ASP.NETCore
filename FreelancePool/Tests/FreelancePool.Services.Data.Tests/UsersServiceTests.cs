namespace FreelancePool.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Common;
    using FreelancePool.Data;
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

        [Fact]
        public async Task GetUserIdByEmailReturnsCorrectId()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var expectedResult = usersRepository.All().Where(u => u.Email == "TestUser1@user.bg").Select(u => u.Id).FirstOrDefault();

            Assert.Equal(expectedResult, service.GetUserIdByEmail("TestUser1@user.bg"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("wrongEmail")]
        public async Task GetUserIdByEmailReturnsNullIfEmailIsInvalid(string email)
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Null(service.GetUserIdByEmail(email));
        }

        [Theory]
        [InlineData("TestUser1@user.bg, TestUser8@user.bg")]
        [InlineData("TestUser1@user.bg TestUser8@user.bg")]
        [InlineData("TestUser1@user.bg,  TestUser8@user.bg")]
        public async Task GetUsersIdsFromEmailsStringReturnsCorrectIds(string emails)
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var expectedResult = usersRepository.All().Where(u => u.Email == "TestUser1@user.bg" || u.Email == "TestUser8@user.bg").Select(u => u.Id).ToList();

            Assert.Equal(expectedResult, service.GetUsersIdsFromEmailsString(emails));
        }

        [Theory]
        [InlineData("invalid1@user.bg, invalid2@user.bg")]
        [InlineData("")]
        [InlineData(null)]
        public async Task GetUsersIdsFromEmailsStringReturnsEmptyCollectionIfEmailsAreInvalid(string emails)
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Empty(service.GetUsersIdsFromEmailsString(emails));
        }

        [Fact]
        public async Task GetUsersIdsFromEmailsStringReturnsOnlyIdsForValidMails()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var expextedResult = usersRepository.All().Where(u => u.Email == "TestUser1@user.bg").Select(u => u.Id).ToList();

            Assert.Equal(expextedResult, service.GetUsersIdsFromEmailsString("TestUser1@user.bg, invalid@user.bg"));
        }

        [Fact]
        public async Task ApplyAsyncAddUserToApliedFreelancers()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var userCandidateProjectsRepository = new EfRepository<UserCandidateProject>(dbContext);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await service.ApplyAsync("TestUser1", 1);

            var actualResult = userCandidateProjectsRepository.All().Where(uc => uc.ProjectId == 1).Select(up => up.UserId).FirstOrDefault();

            Assert.Equal("TestUser1", actualResult);
        }

        [Fact]
        public async Task GetUserByIdReturnsCorrectUser()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var expectedResult = usersRepository.All().Where(u => u.Id == "TestUser1").Select(u => u.Id).FirstOrDefault();

            Assert.Equal(expectedResult, service.GetUserById<FreelancerViewModel>("TestUser1").Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalidId")]
        public async Task GetUserByIdReturnsNullIfIdIsInvalid(string id)
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);


            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Null(service.GetUserById<FreelancerViewModel>(id));
        }

        [Fact]
        public async Task CreateAProfileAsyncAddUserToFreelancerRole()
        {
            MapperInitializer.InitializeMapper();

            var userManager = this.GetUserManagerMock();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            List<int> categoriesIds = new List<int> { 3 };

            await service.CreateAProfileAsync("TestUser9", "TestUser1", "photo", "TestUser9@user.bg", "some summary", "phone", categoriesIds);

            var user = usersRepository.All().Where(u => u.UserName == "TestUser9").FirstOrDefault();

            var isInRole = await userManager.Object.IsInRoleAsync(user, GlobalConstants.FreelancerRoleName);

            Assert.True(isInRole);
        }

        [Fact]
        public async Task CreateAProfileChangesUsersCharacteristics()
        {
            MapperInitializer.InitializeMapper();

            var userManager = this.GetUserManagerMock();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            List<int> categoriesIds = new List<int> { 3 };

            await service.CreateAProfileAsync("TestUser1", "TestChange", "photo", "TestChange@user.bg", "some summary", "phone", categoriesIds);

            var user = usersRepository.All().Where(u => u.UserName == "TestChange").FirstOrDefault();

            var expextedResult = new List<string> { "TestChange", "photo", "TestChange@user.bg", "some summary", "phone" };

            var actualResult = new List<string>(new string[] { user.UserName, user.PhotoUrl, user.Email, user.Summary, user.PhoneNumber });

            Assert.Equal(expextedResult, actualResult);
        }

        [Fact]
        public async Task CreateAProfileAddCategoriesToUser()
        {
            MapperInitializer.InitializeMapper();

            var userManager = this.GetUserManagerMock();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            List<int> categoriesIds = new List<int> { 3 };

            await service.CreateAProfileAsync("TestUser1", "TestChange", "photo", "TestChange@user.bg", "some summary", "phone", categoriesIds);

            var user = usersRepository.All().Where(u => u.UserName == "TestChange").FirstOrDefault();

            Assert.Contains(user.UserCategories, uc => uc.CategoryId == 3);
        }

        [Fact]
        public async Task RateFreelancerAsyncIncreasesStars()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await service.RateFreelancerAsync("TestUser1", "TestUser2", 1, " ");

            var user = usersRepository.All().Where(u => u.Id == "TestUser2").FirstOrDefault();

            Assert.Equal(1, user.Stars);
        }

        [Fact]
        public async Task RateFreelancerAsyncDecreasesStars()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await service.RateFreelancerAsync("TestUser1", "TestUser2", -1, " ");

            var user = usersRepository.All().Where(u => u.Id == "TestUser2").FirstOrDefault();

            Assert.Equal(-1, user.Stars);
        }

        [Fact]
        public async Task RateFreelancerAsyncAddsRecommendation()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await service.RateFreelancerAsync("TestUser1", "TestUser2", -1, "Recommendation");

            var user = usersRepository.All().Where(u => u.Id == "TestUser2").FirstOrDefault();

            Assert.Single(user.Recommendations);
        }

        [Fact]
        public async Task GetUserByNameReturnsCorrectUser()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var expectedResult = usersRepository.All().Where(u => u.UserName == "TestUser1").FirstOrDefault();

            Assert.Equal(expectedResult.Id, service.GetUserByName<FreelancerViewModel>("TestUser1").Id);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("invalidName")]
        public async Task GetUserByNameReturnsNullIfNameIsInvalid(string name)
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Null(service.GetUserByName<FreelancerViewModel>(name));
        }

        private UsersService InitializeService(EfDeletableEntityRepository<ApplicationUser> usersRepository, ApplicationDbContext dbContext)
        {
            var userCandidateProjectsRepository = new EfRepository<UserCandidateProject>(dbContext);
            var categoryUsersRepository = new EfRepository<CategoryUser>(dbContext);
            var recommendationsRepository = new EfDeletableEntityRepository<Recommendation>(dbContext);
            var userManager = this.GetUserManagerMock();

            var service = new UsersService(usersRepository, userCandidateProjectsRepository, categoryUsersRepository, recommendationsRepository, userManager.Object);

            return service;
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), GlobalConstants.FreelancerRoleName))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

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
                Email = "TestUser1@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 1, 8 }, "TestUser1", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser2",
                UserName = "TestUser2",
                Email = "TestUser2@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 1, 2 }, "TestUser2", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser3",
                UserName = "TestUser3",
                Email = "TestUser3@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 5, 8 }, "TestUser3", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser4",
                UserName = "TestUser4",
                Email = "TestUser4@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 5 }, "TestUser4", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser5",
                UserName = "TestUser5",
                Email = "TestUser5@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 2, 8 }, "TestUser5", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser6",
                UserName = "TestUser6",
                Email = "TestUser6@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 1, 5 }, "TestUser6", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser7",
                UserName = "TestUser7",
                Email = "TestUser7@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 8 }, "TestUser7", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser8",
                UserName = "TestUser8",
                Email = "TestUser8@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 2, 5 }, "TestUser8", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser9",
                UserName = "TestUser9",
                Email = "TestUser9@user.bg",
            });

            await AddUserToCategoriesAsync(new int[] { 2 }, "TestUser9", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser10",
                UserName = "TestUser10",
                Email = "TestUser10@user.bg",
            });
            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser11",
                UserName = "TestUser11",
                Email = "TestUser11@user.bg",
            });
            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser12",
                UserName = "TestUser12",
                Email = "TestUser12@user.bg",
            });

            await dbContext.SaveChangesAsync();

            //Seeding some projects
            dbContext.Projects.Add(new Project
            {
                Id = 1,
                Title = "First test title",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 2,
                Title = "Second test title",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 3,
                Title = "Third test title",
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
