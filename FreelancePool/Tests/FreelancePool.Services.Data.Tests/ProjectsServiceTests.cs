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
    using FreelancePool.Web.ViewModels.Components;
    using FreelancePool.Web.ViewModels.Projects;
    using Xunit;

    public class ProjectsServiceTests
    {
        [Fact]
        public async Task CloseAsyncSetsProjectsExecutor()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            await service.CloseAsync(1, "TestUser1");

            var result = projectsRepository.All().Where(p => p.Id == 1).Select(p => p.ExecutorId).FirstOrDefault();

            Assert.Equal("TestUser1", result);
        }

        [Fact]
        public async Task CloseAsyncSetsProjectsStatusToFinished()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            await service.CloseAsync(1, "someExec");

            var result = projectsRepository.All().Where(p => p.Id == 1).Select(p => p.Status).FirstOrDefault();

            Assert.Equal(3, (int)result);
        }

        [Fact]
        public async Task CreateAsyncReturnsProjectsId()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            var actualResult = await service.CreateAsync("Title", "description", "authorId", new List<int> { 1, 2 }, new List<string> { "TestUser1", "TestUser2" });

            var expectedResult = projectsRepository.All().Where(p => p.Title == "Title").Select(p => p.Id).FirstOrDefault();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task CreateAsyncSetsProjectToCategories()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            var categoriesIds = new List<int> { 1, 2 };

            await service.CreateAsync("Title", "description", "authorId", categoriesIds, new List<string> { "TestUser1", "TestUser2" });

            var actualResult = projectsRepository.All().Where(p => p.Title == "Title").Select(p => p.ProjectCategories.Select(pc => pc.CategoryId)).FirstOrDefault();

            Assert.Equal(categoriesIds, actualResult);
        }

        [Fact]
        public async Task GetByIdReturnsCorrectProject()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            var actualResult = service.GetById<CloseProjectViewModel>(1).Id;
            var expectedResult = projectsRepository.All().Where(p => p.Id == 1).Select(p => p.Id).FirstOrDefault();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetByIdReturnsNullIdIdIsInvalid()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            Assert.Null(service.GetById<CloseProjectViewModel>(0));
        }

        [Fact]
        public async Task GetLastProjectsReturnsEightEntities()
        {
            MapperInitializer.InitializeMapper();
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            var user = dbContext.Users.Where(u => u.Id == "TestUser1").FirstOrDefault();

            IEnumerable<ProjectViewModel> projects = service.GetLastProjects<ProjectViewModel>(user);

            Assert.Equal(8, projects.Count());
        }

        [Fact]
        public async Task GetAuthorIdReturnsCorrectValue()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            Assert.Equal("TestUser2", service.GetAuthorId(4));
        }

        [Fact]
        public async Task GetAuthorIdReturnsNullIfIdIsInvalid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            Assert.Null(service.GetAuthorId(0));
        }

        [Fact]
        public async Task GetAllReturnsAllOpenProjects()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            var actualResult = service.GetAll<ProjectViewModel>().Count();

            var expectedResult = projectsRepository.All().Where(p => (int)p.Status == 1).ToList().Count();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetAllReturnsProjectsOrderedByCreation()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            List<int> expectedResult = new List<int> { 4, 2, 6 };

            var actualResult = service.GetAll<ProjectViewModel>().Select(p => p.Id).ToList().Take(3);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public async Task GetMostWantedReturnsFiveEntities()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            var actualResult = service.GetMostWanted<ProjectTitleAndApplicantsViewModel>().Count();

            Assert.Equal(5, actualResult);
        }

        [Fact]
        public async Task GetMostWantedReturnsProjectsWithMostApplicants()
        {
            MapperInitializer.InitializeMapper();

            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);
            var service = this.InitializeService(projectsRepository, dbContext);

            var expextedResult = new List<int>() { 2, 3 };

            var actualResult = service.GetMostWanted<ProjectTitleAndApplicantsViewModel>().Select(p => p.Id).ToList().Take(2);

            Assert.Equal(expextedResult, actualResult);
        }

        [Fact]
        public async Task GetTitleByIdReturnsCorrectValue()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);

            var service = this.InitializeService(projectsRepository, dbContext);

            Assert.Equal("Second test title", service.GetTitleById(2));
        }

        [Fact]
        public async Task GetTitleByIdReturnsNullIfIdIsInvalid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);

            var service = this.InitializeService(projectsRepository, dbContext);

            Assert.Null(service.GetTitleById(0));
        }

        [Fact]
        public async Task DeleteThrowsArgumentNullExceptionIfProjectTitleIsInvalid()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);

            var service = this.InitializeService(projectsRepository, dbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.Delete("invalidTitle", "TestUser2@user.bg"));
        }

        [Theory]
        [InlineData("TestUser1@user.bg")]
        [InlineData("InvalidEmail")]
        public async Task DeleteThrowsArgumentNullExceptionIfAuthorsEmailIsIncorrect(string authorEmail)
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);

            var service = this.InitializeService(projectsRepository, dbContext);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.Delete("First test title", authorEmail));
        }

        [Fact]
        public async Task DeleteSetsProjectToDeleted()
        {
            var dbContext = ApplicationDbContextInMemoryFactory.InitializeContext();

            await SeedDataAsync(dbContext);

            var projectsRepository = new EfDeletableEntityRepository<Project>(dbContext);

            var service = this.InitializeService(projectsRepository, dbContext);

            await service.Delete("First test title", "TestUser2@user.bg");

            var result = projectsRepository.AllWithDeleted().Where(p => p.Title == "First test title").FirstOrDefault().IsDeleted;

            Assert.True(result);
        }

        private static async Task SeedDataAsync(ApplicationDbContext dbContext)
        {
            dbContext.UsersCandidateProjects.Add(new UserCandidateProject
            {
                UserId = "TestUser1",
                ProjectId = 2,
            });

            dbContext.UsersCandidateProjects.Add(new UserCandidateProject
            {
                UserId = "TestUser2",
                ProjectId = 2,
            });

            dbContext.UsersCandidateProjects.Add(new UserCandidateProject
            {
                UserId = "TestUser1",
                ProjectId = 3,
            });

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

            // Seeding some projects
            dbContext.Projects.Add(new Project
            {
                Id = 1,
                Title = "First test title",
                Status = ProjectStatus.Open,
                AuthorId = "TestUser2",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 2,
                Title = "Second test title",
                CreatedOn = DateTime.UtcNow.AddSeconds(20),
                Status = ProjectStatus.Open,
                AuthorId = "TestUser2",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 3,
                Title = "Third test title",
                Status = ProjectStatus.Open,
                AuthorId = "TestUser2",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 4,
                Title = "Forth test title",
                CreatedOn = DateTime.UtcNow.AddSeconds(50),
                Status = ProjectStatus.Open,
                AuthorId = "TestUser2",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 5,
                Title = "Fifth test title",
                Status = ProjectStatus.Open,
                AuthorId = "TestUser1",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 6,
                Title = "Sixth test title",
                CreatedOn = DateTime.UtcNow.AddSeconds(5),
                Status = ProjectStatus.Open,
                AuthorId = "TestUser1",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 7,
                Title = "Seventh test title",
                Status = ProjectStatus.Open,
                AuthorId = "TestUser1",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 8,
                Title = "Eighth test title",
                Status = ProjectStatus.Open,
                AuthorId = "TestUser1",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 9,
                Title = "Nine test title",
                Status = ProjectStatus.Open,
                AuthorId = "TestUser1",
            });

            dbContext.Projects.Add(new Project
            {
                Id = 10,
                Title = "Ten test title",
                AuthorId = "TestUser1",
            });

            await dbContext.SaveChangesAsync();

            // Seeding users
            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser1",
                UserName = "TestUser1",
                Email = "TestUser1@user.bg",
                Stars = 1,
            });

            dbContext.Users.Add(new ApplicationUser
            {
                Id = "TestUser2",
                UserName = "TestUser2",
                Email = "TestUser2@user.bg",
                Stars = 6,
            });

            await dbContext.SaveChangesAsync();
        }

        private ProjectsService InitializeService(EfDeletableEntityRepository<Project> projectsRepository, ApplicationDbContext dbContext)
        {
            var projectCategoryRepository = new EfRepository<CategoryProject>(dbContext);
            var projectOfferUserRepository = new EfRepository<ProjectOfferUser>(dbContext);

            var service = new ProjectsService(projectsRepository, projectCategoryRepository, projectOfferUserRepository);

            return service;
        }
    }
}
