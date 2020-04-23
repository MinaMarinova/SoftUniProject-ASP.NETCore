namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProjectOfferUsersService
    {
        Task CreateAsync(int projectId, IEnumerable<string> usersIds);
    }
}
