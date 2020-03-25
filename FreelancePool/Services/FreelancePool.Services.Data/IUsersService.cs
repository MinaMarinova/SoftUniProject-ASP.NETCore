namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;

    using FreelancePool.Data.Models;

    public interface IUsersService
    {
        IEnumerable<T> GetRandomEightUsersByCategories<T>(ApplicationUser user);
    }
}
