namespace FreelancePool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<UserCandidateProject> userCandidateProjectsRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IRepository<UserCandidateProject> userCandidateProjectsRepository)
        {
            this.userRepository = userRepository;
            this.userCandidateProjectsRepository = userCandidateProjectsRepository;
        }

        public IEnumerable<T> GetRandomEightUsers<T>(ApplicationUser user)
        {
            var freelancers = this.userRepository.All().Where(u => u.UserCategories.Count > 0);

            if (user != null && user.UserCategories.Count > 0)
            {
                List<string> categories = user.UserCategories.Select(uc => uc.Category.Name).ToList();

                freelancers = this.userRepository.All()
                    .Where(u => u.Id != user.Id && u.UserCategories.Select(uc => uc.Category.Name).Any(x => categories.Contains(x)));
            }

            if (freelancers.Count() > 8)
            {
                freelancers = freelancers.Skip(GetNumberToSkip(this.userRepository)).Take(8);
            }

            return freelancers.To<T>().ToList();
        }

        public string GetUserIdByEmail(string email)
        {
            string userId = this.userRepository.All()
                .Where(u => u.Email == email)
                .Select(u => u.Id)
                .FirstOrDefault();

            return userId;
        }

        public List<string> GetUsersEmailsFromString(string usersEmails)
        {
            var usersIds = new List<string>();

            if (!string.IsNullOrWhiteSpace(usersEmails))
            {
                var usersEmailsCollection = usersEmails.Split(
                    new[] { ',', ' ' },
                    StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (var email in usersEmailsCollection)
                {
                    var userId = this.GetUserIdByEmail(email);

                    if (userId != null)
                    {
                        usersIds.Add(userId);
                    }
                }
            }

            return usersIds;
        }

        public async Task ApplyAsync(string userId, int projectId)
        {
            var userCandidateProject = new UserCandidateProject
            {
                UserId = userId,
                ProjectId = projectId,
            };

            await this.userCandidateProjectsRepository.AddAsync(userCandidateProject);
            await this.userCandidateProjectsRepository.SaveChangesAsync();
        }

        private static int GetNumberToSkip(IDeletableEntityRepository<ApplicationUser> repository)
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, repository.All().Where(u => u.UserCategories.Count > 0).Count() - 7);
            return toSkip;
        }

        
    }
}
