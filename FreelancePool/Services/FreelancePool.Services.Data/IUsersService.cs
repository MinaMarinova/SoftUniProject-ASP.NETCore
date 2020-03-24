namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;

    public interface IUsersService
    {
        IEnumerable<T> GetRandomEightUsers<T>();

        IEnumerable<T> GetRandomEightUsersByCategories<T>();
    }
}
