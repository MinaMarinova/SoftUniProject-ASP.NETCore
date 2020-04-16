namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;

    public interface ICategoriesService
    {
        int GetCategoryIdByName(string categoryName);

        IEnumerable<T> GetAll<T>();

        string GetNameById(int id);
    }
}
