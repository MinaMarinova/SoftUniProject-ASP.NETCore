namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;

    public class ProjectOfferUsersService : IProjectOfferUsersService
    {
        private readonly IRepository<ProjectOfferUser> projectOfferUsersRepository;

        public ProjectOfferUsersService(IRepository<ProjectOfferUser> projectOfferUsersRepository)
        {
            this.projectOfferUsersRepository = projectOfferUsersRepository;
        }

        public async Task CreateAsync(int projectId, IEnumerable<string> usersIds)
        {
            foreach (var userId in usersIds)
            {
                var projectOfferUser = new ProjectOfferUser
                {
                    ProjectId = projectId,
                    UserId = userId,
                };

                await this.projectOfferUsersRepository.AddAsync(projectOfferUser);
                await this.projectOfferUsersRepository.SaveChangesAsync();
            }
        }
    }
}
