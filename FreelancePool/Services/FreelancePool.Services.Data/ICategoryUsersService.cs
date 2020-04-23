namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryUsersService
    {
        Task CreateAsync(string userId, ICollection<int> categoriesId);
    }
}
