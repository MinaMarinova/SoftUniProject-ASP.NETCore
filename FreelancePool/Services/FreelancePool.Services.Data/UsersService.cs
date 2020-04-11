namespace FreelancePool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FreelancePool.Common;
    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using Microsoft.AspNetCore.Identity;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<UserCandidateProject> userCandidateProjectsRepository;
        private readonly IRepository<CategoryUser> categoryUsersRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IRepository<UserCandidateProject> userCandidateProjectsRepository,
            IRepository<CategoryUser> categoryUsersRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepository;
            this.userCandidateProjectsRepository = userCandidateProjectsRepository;
            this.categoryUsersRepository = categoryUsersRepository;
            this.userManager = userManager;
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

        public T GetUserById<T>(string id)
        {
            var user = this.userRepository.All()
                .Where(u => u.Id == id)
                .To<T>()
                .FirstOrDefault();

            return user;
        }

        public async Task CreateAProfileAsync(string userId, string userName, string photoUrl, string email, string summary, string phoneNumber, ICollection<int> categoriesId)
        {
            var user = this.userRepository.All().Where(u => u.Id == userId).FirstOrDefault();

            user.UserName = userName;
            user.NormalizedUserName = userName.ToUpper();

            if (photoUrl != null)
            {
                user.PhotoUrl = photoUrl;
            }

            user.Email = email;
            user.Summary = summary;
            user.PhoneNumber = phoneNumber;

            await this.userManager.AddToRoleAsync(user, GlobalConstants.FreelancerRoleName);

            await this.userRepository.SaveChangesAsync();

            foreach (var categoryId in categoriesId)
            {
                var categoryUser = new CategoryUser
                {
                    CategoryId = categoryId,
                    UserId = userId,
                };

                await this.categoryUsersRepository.AddAsync(categoryUser);
                await this.categoryUsersRepository.SaveChangesAsync();
            }
        }

        private static int GetNumberToSkip(IDeletableEntityRepository<ApplicationUser> repository)
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, repository.All().Where(u => u.UserCategories.Count > 0).Count() - 7);
            return toSkip;
        }


    }
}
