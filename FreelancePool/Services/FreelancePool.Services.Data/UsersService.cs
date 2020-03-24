namespace FreelancePool.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FreelancePool.Data.Common.Repositories;
    using FreelancePool.Data.Models;
    using FreelancePool.Services.Mapping;
    using Microsoft.AspNetCore.Identity;

    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private int toSkip;
        public UsersService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IEnumerable<T> GetRandomEightUsers<T>()
        {
            if (this.userManager.Users.Count() > 8)
            {
                Random rand = new Random();
                this.toSkip = rand.Next(1, this.userManager.Users.Count() - 8);

                IQueryable users = this.userManager.Users.Skip(this.toSkip).Take(8);
                return users.To<T>().ToList();
            }
            else
            {
                IQueryable users = this.userManager.Users;
                return users.To<T>().ToList();
            }
        }

        public IEnumerable<T> GetRandomEightUsersByCategories<T>()
        {
            return null;
        }
    }
}
