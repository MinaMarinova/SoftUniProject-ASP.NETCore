namespace FreelancePool.Services.Data.Tests
{
    using System;
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

            await service.CreateAProfileAsync("TestUser9", "TestUser1", "photo", "TestUser9@user.bg", "some summary", "phone");

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

            await service.CreateAProfileAsync("TestUser1", "TestChange", "photo", "TestChange@user.bg", "some summary", "phone");

            var user = usersRepository.All().Where(u => u.UserName == "TestChange").FirstOrDefault();

            var expextedResult = new List<string> { "TestChange", "photo", "TestChange@user.bg", "some summary", "phone" };

            var actualResult = new List<string>(new string[] { user.UserName, user.PhotoUrl, user.Email, user.Summary, user.PhoneNumber });

            Assert.Equal(expextedResult, actualResult);
        }

        [Fact]
        public async Task RateFreelancerAsyncIncreasesStars()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await service.RateFreelancerAsync("TestUser1", "TestUser8", 1, " ");

            var user = usersRepository.All().Where(u => u.Id == "TestUser8").FirstOrDefault();

            Assert.Equal(1, user.Stars);
        }

        [Fact]
        public async Task RateFreelancerAsyncDecreasesStars()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await service.RateFreelancerAsync("TestUser1", "TestUser8", -1, " ");

            var user = usersRepository.All().Where(u => u.Id == "TestUser8").FirstOrDefault();

            Assert.Equal(-1, user.Stars);
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
        [InlineData("Invalidname")]
        public async Task GetUserByNameReturnsNullIfNameIsInvalid(string name)
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Null(service.GetUserByName<FreelancerViewModel>(name));
        }

        [Fact]
        public async Task GetAllReturnsAllUsersAssignedToCategory()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var expectedResult = usersRepository.All().Where(u => u.UserCategories.Any()).ToList();

            var actualResult = service.GetAll<FreelancerViewModel>();

            Assert.Equal(expectedResult.Count(), actualResult.Count());
        }

        [Fact]
        public async Task GetTopGetSixFreelancers()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Equal(6, service.GetTop<FreelancerViewModel>().Count());
        }

        [Fact]
        public async Task GetTopGetFreelancersWithMaximumStars()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var expectedResult = new List<string> { "TestUser2", "TestUser4" };
            var actualResult = service.GetTop<FreelancerViewModel>().Take(2).Select(f => f.Id).ToList();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetRecentReturnsThreeFreelancers()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Equal(3, service.GetRecent<FreelancerViewModel>().Count());
        }

        [Fact]
        public async Task GetRecentReturnsRecentlyJoinedFreelancers()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var expectedResult = usersRepository.All().OrderByDescending(f => f.CreatedOn).Select(x => x.Id).ToList().Take(3);

            var actualResult = service.GetRecent<FreelancerViewModel>().Select(u => u.Id).ToList();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetUserEmailByIdReturnsCorrectData()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Equal("TestUser1@user.bg", service.GetUserEmailById("TestUser1"));
        }

        [Fact]
        public async Task GetUserEmailByIdReturnsNullIfIdIsInvalid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Null(service.GetUserEmailById("invalidId"));
        }

        [Fact]
        public async Task AddAdminThrowsArgumentExceptionIfUserNameIsInUse()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAdmin("TestUser1", "someEmail", "password"));
        }

        [Fact]
        public async Task AddAdminThrowsArgumentExceptionIfEmailIsInUse()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await Assert.ThrowsAsync<ArgumentException>(() => service.AddAdmin("User", "TestUser1@user.bg", "password"));
        }

        [Fact]
        public async Task AddAdminCreatesNewUser()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.True(await service.AddAdmin("New user", "user@user.bg", "password"));
        }

        [Fact]
        public async Task AddAdminAddsTheNewUserToAdminRole()
        {
            var userManager = this.GetUserManagerMock();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await service.AddAdmin("New user", "user@user.bg", "password");

            var user = usersRepository.All().Where(u => u.UserName == "New user").FirstOrDefault();

            var result = await userManager.Object.IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);

            Assert.True(result);
        }

        [Fact]
        public async Task RemoveAdminAsyncThrowsArgumentNullExceptionIfEmailIsInvalid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.RemoveAdminAsync("wrongUser", GlobalConstants.AdministratorRoleName));
        }

        [Fact]
        public async Task RemoveAdminAsyncThrowsArgumentNullExceptionIfUserIsNotAdmin()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            var user = usersRepository.All().Where(u => u.UserName == "TestUser5").FirstOrDefault();

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.RemoveAdminAsync("TestUser5@user.bg", GlobalConstants.AdministratorRoleName));
        }

        [Fact]
        public async Task RemoveUserAsyncThrowsArgumentNullExceptionIfEmailIsInvalid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.RemoveUserAsync("wrongUser"));
        }

        [Fact]
        public async Task RemoveUserAsyncSetsIsDeletedUserToTrue()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            await service.RemoveUserAsync("TestUser4@user.bg");

            var result = usersRepository.All().Where(u => u.UserName == "UserTest4@user.bg").FirstOrDefault();

            Assert.Null(result);
        }

        [Fact]
        public async Task RemoveUserAsyncReturnsCorrectUserName()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            var service = this.InitializeService(usersRepository, dbContext);

            Assert.Equal("TestUser4", await service.RemoveUserAsync("TestUser4@user.bg"));
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
                Stars = 1,
                CreatedOn = DateTime.UtcNow.AddSeconds(2),
            });

            await AddUserToCategoriesAsync(new int[] { 1, 8 }, "TestUser1", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser2",
                UserName = "TestUser2",
                Email = "TestUser2@user.bg",
                Stars = 6,
                CreatedOn = DateTime.UtcNow.AddSeconds(10),
            });

            await AddUserToCategoriesAsync(new int[] { 1, 2 }, "TestUser2", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser3",
                UserName = "TestUser3",
                Email = "TestUser3@user.bg",
                Stars = 3,
                CreatedOn = DateTime.UtcNow.AddSeconds(5),
            });

            await AddUserToCategoriesAsync(new int[] { 5, 8 }, "TestUser3", dbContext);

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser4",
                UserName = "TestUser4",
                Email = "TestUser4@user.bg",
                Stars = 4,
                CreatedOn = DateTime.UtcNow.AddSeconds(1),
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

            // Seeding some projects
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

        private UsersService InitializeService(EfDeletableEntityRepository<ApplicationUser> usersRepository, ApplicationDbContext dbContext)
        {
            var userManager = this.GetUserManagerMock();

            var service = new UsersService(usersRepository, userManager.Object);

            return service;
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(x => x.IsInRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            return userManagerMock;
        }
    }
}
