namespace FreelancePool.Services.Data
{
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;

    public class UserCandidateProjectsService : IUserCandidateProjectsService
    {
        private readonly IRepository<UserCandidateProject> repository;

        public UserCandidateProjectsService(IRepository<UserCandidateProject> repository)
        {
            this.repository = repository;
        }

        public async Task ApplyAsync(string userId, int projectId)
        {
            var userCandidateProject = new UserCandidateProject
            {
                UserId = userId,
                ProjectId = projectId,
            };

            await this.repository.AddAsync(userCandidateProject);
            await this.repository.SaveChangesAsync();
        }
    }
}
