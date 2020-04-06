namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FreelancePool.Data.Models;

    public interface IUsersService
    {
        IEnumerable<T> GetRandomEightUsers<T>(ApplicationUser user);

        string GetUserIdByEmail(string email);

        List<string> GetUsersEmailsFromString(string usersEmails);

        Task ApplyAsync(string userId, int projectId);

        T GetUserById<T>(string id);
    }
}
