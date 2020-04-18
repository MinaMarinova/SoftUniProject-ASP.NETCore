namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        int GetCategoryIdByName(string categoryName);

        IEnumerable<T> GetAll<T>();

        string GetNameById(int id);

        Task<int> Add(string name, string iconUrl);

        Task<int> Delete(string name);

        Task<int> Edit(string currentName, string name);
    }
}
