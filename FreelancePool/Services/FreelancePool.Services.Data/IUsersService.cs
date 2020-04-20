namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;

    public interface IUsersService
    {
        IEnumerable<T> GetRandomFreelancers<T>(ApplicationUser user);

        string GetUserIdByEmail(string email);

        List<string> GetUsersEmailsFromString(string usersEmails);

        Task ApplyAsync(string userId, int projectId);

        T GetUserById<T>(string id);

        T GetUserByName<T>(string userName);

        Task CreateAProfileAsync(string userId, string userName, string photoUrl, string email, string summary, string phoneNumber, ICollection<int> categoriesId);

        Task RateFreelancerAsync(string authorId, string executorId, int starGivenOrTaken, string recommendation);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetTop<T>();

        IEnumerable<T> GetRecent<T>();

        string GetUserEmailById(string userId);

        Task<bool> AddAdmin(string userName, string email, string password);

        Task<string> RemoveAdmin(string email, string role);

        Task<string> RemoveUser(string email);
    }
}
