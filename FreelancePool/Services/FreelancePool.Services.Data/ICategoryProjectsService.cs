namespace FreelancePool.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryProjectsService
    {
        Task CreateAsync(int projectId, ICollection<int> categoriesId);
    }
}
