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
        private const int NumberOfTopFreelancers = 6;
        private const int NumberOfRandomFreelancers = 8;

        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<UserCandidateProject> userCandidateProjectsRepository;
        private readonly IRepository<CategoryUser> categoryUsersRepository;
        private readonly IDeletableEntityRepository<Recommendation> recommendationsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IRepository<UserCandidateProject> userCandidateProjectsRepository,
            IRepository<CategoryUser> categoryUsersRepository,
            IDeletableEntityRepository<Recommendation> recommendationsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepository;
            this.userCandidateProjectsRepository = userCandidateProjectsRepository;
            this.categoryUsersRepository = categoryUsersRepository;
            this.recommendationsRepository = recommendationsRepository;
            this.userManager = userManager;
        }

        public IEnumerable<T> GetRandomEightUsers<T>(ApplicationUser user)
        {
            var freelancers = this.userRepository.All()
                .Where(u => u.UserCategories.Count > 0);

            if (user != null && user.UserCategories.Count > 0)
            {
                List<string> categories = user.UserCategories.Select(uc => uc.Category.Name).ToList();

                freelancers = this.userRepository.All()
                    .Where(u => u.Id != user.Id && u.UserCategories.Select(uc => uc.Category.Name).Any(x => categories.Contains(x)));
            }

            if (freelancers.Count() > 8)
            {
                freelancers = freelancers.Skip(GetNumberToSkip(freelancers.Count())).Take(8);
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
            var user = this.userRepository.All()
                .Where(u => u.Id == userId)
                .FirstOrDefault();

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

        public async Task RateFreelancerAsync(string authorId, string executorId, int starGivenOrTaken, string recommendation)
        {
            var freelancer = this.userRepository.All()
                .Where(u => u.Id == executorId)
                .FirstOrDefault();

            if (freelancer != null)
            {
                freelancer.Stars += starGivenOrTaken;
                await this.userRepository.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(recommendation))
                {
                    var recommendationToAdd = new Recommendation
                    {
                        AuthorId = authorId,
                        RecipientId = executorId,
                        Content = recommendation,
                    };

                    await this.recommendationsRepository.AddAsync(recommendationToAdd);
                    await this.recommendationsRepository.SaveChangesAsync();
                }
            }
        }

        public T GetUserByName<T>(string userName)
        {
            return this.userRepository.All().Where(u => u.UserName == userName).To<T>().FirstOrDefault();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var users = this.userRepository.All()
                .Where(u => u.UserCategories.Count() > 0);

            return users.To<T>().ToList();
        }

        public IEnumerable<T> GetTop<T>()
        {
            var topFreelancers = this.userRepository.All()
                .Where(u => u.UserCategories.Count() > 0)
                .OrderByDescending(u => u.Stars)
                .ThenByDescending(u => u.Recommendations.Count())
                .Take(NumberOfTopFreelancers);

            return topFreelancers.To<T>().ToList();
        }

        private static int GetNumberToSkip(int freelancersCount)
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, freelancersCount - (NumberOfRandomFreelancers - 1));

            return toSkip;
        }
    }
}
