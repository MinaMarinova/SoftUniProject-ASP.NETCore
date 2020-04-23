namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using FreelancePool.Data.Models;

    public interface IUsersService
    {
        IEnumerable<T> GetRandomFreelancers<T>(ApplicationUser user);

        string GetUserIdByEmail(string email);

        List<string> GetUsersIdsFromEmailsString(string usersEmails);

        T GetUserById<T>(string id);

        T GetUserByName<T>(string userName);

        Task CreateAProfileAsync(string userId, string userName, string photoUrl, string email, string summary, string phoneNumber);

        Task RateFreelancerAsync(string authorId, string executorId, int starGivenOrTaken, string recommendation);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetTop<T>();

        IEnumerable<T> GetRecent<T>();

        string GetUserEmailById(string userId);

        Task<bool> AddAdmin(string userName, string email, string password);

        Task<string> RemoveAdminAsync(string email, string role);

        Task<string> RemoveUserAsync(string email);
    }
}
