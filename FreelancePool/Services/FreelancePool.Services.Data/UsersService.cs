namespace FreelancePool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<T> GetRandomEightUsersByCategories<T>(ApplicationUser user)
        {
            var freelancers = this.userRepository.All();

            if (user != null && user.UserCategories.Count > 0)
            {
                freelancers = this.userRepository.All()
                    .Where(u => u.UserCategories.Intersect(user.UserCategories).Any());
            }

            if (freelancers.Count() > 8)
            {
                freelancers = freelancers.Skip(GetNumberToSkip(this.userRepository)).Take(8);
            }

            return freelancers.To<T>().ToList();
        }

        private static int GetNumberToSkip(IDeletableEntityRepository<ApplicationUser> repository)
        {
            Random rand = new Random();
            int toSkip = rand.Next(0, repository.All().Count() - 7);
            return toSkip;
        }
    }
}
